// NAnt - A .NET build tool
// Copyright (C) 2001-2002 Gerry Shaw
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

// Ian MacLean (ian@maclean.ms)

using System;
using System.Reflection;
using System.Xml;

using SourceForge.NAnt.Attributes;

namespace SourceForge.NAnt {

    /// <summary>Models a NAnt XML element in the build file.</summary>
    /// <remarks>
    ///   <para>Automatically validates attributes in the element based on Attribute settings in the derived class.</para>
    /// </remarks>
    public class Element {

        Location _location = Location.UnknownLocation;
        Project _project = null;

        /// <summary>The default contstructor.</summary>
        public Element(){
        }

        /// <summary>A copy contstructor.</summary>
        protected Element(Element e) : this() {
            this._location = e._location;
            this._project = e._project;
        }

        /// <summary><see cref="Location"/> in the build file where the element is defined.</summary>
        protected Location Location {
            get { return _location; }
            set { _location = value; }
        }

        /// <summary>Name of the XML element used to initialize this element.</summary>
        public virtual string Name {
            get {
                ElementNameAttribute elementNameAttribute = (ElementNameAttribute) 
                    Attribute.GetCustomAttribute(GetType(), typeof(ElementNameAttribute));

                string name = null;
                if (elementNameAttribute != null) {
                    name = elementNameAttribute.Name;
                }
                return name;
            }
        }

        /// <summary><see cref="Project"/> this element belongs to.</summary>
        public Project Project {
            get { return _project; }
            set { _project = value; }
        }
     
/*
        See InitializeAttributes for details about these functions and why
        they are commentted out for now.

        protected virtual void ValidateAttribute(string attributeName) {
            Type currentType = GetType();
            while (currentType != typeof(object)) {
                // iterate over each property looking for BuildAttribute attributes
                PropertyInfo[] propertyInfoArray = currentType.GetProperties(BindingFlags.Public|BindingFlags.Instance);
                foreach (PropertyInfo propertyInfo in propertyInfoArray ) {
                    // get the BuildAttribute attribute
                    BuildAttributeAttribute buildAttribute = (BuildAttributeAttribute) 
                        Attribute.GetCustomAttribute(propertyInfo, typeof(BuildAttributeAttribute));

                    if (buildAttribute != null && buildAttribute.Name == attributeName) {
                        // found a handler for this attribute
                        return;
                    }
                }

                // try looking in the super class for BuildAttribute attributes
                currentType = currentType.BaseType;
            }

            // If we haven't found a BuildAttribute then the attribute is unknown
            // unless this is a special case and the class forgot to override this
            // method and provide a check for it.
            throw new BuildException(String.Format("Unknown attribute '{0}'", attributeName), Location);
        }

        protected virtual void ValidateAttributes(XmlNode elementNode) {
            foreach (XmlNode attributeNode in elementNode.Attributes) {
                ValidateAttribute(attributeNode.Name);
            }
        }
*/

        /// <summary>Initializes all build attributes.</summary>
        protected void InitializeAttributes(XmlNode elementNode) {
            // This is a bit of a monster function but if you look at it 
            // carefully this is what it does:
            // * Go down the inheritance tree to find the private fields in the object.
            // * Looking for task attributes to initialize.
            // * For each BuildAttribute try to find the xml attribute that corresponds to it.
            // * Next process all the nested elements, same idea, look at what is supposed to
            //   be there from the attributes on the class/properties and then get
            //   the values from the xml node to set the class properties.
            // * Note that we also go down the inheritance tree so we can pick up BuildAttribute attributes
            //   from super classes as well.

            // This isn't going to work without a LOT of work in defining all the
            // attributes elements initialize.  I think a better way might be to
            // use an automatically generated schema.
            //ValidateAttributes(elementNode);

            Type currentType = GetType();
            while (currentType != typeof(object)) {
                PropertyInfo[] propertyInfoArray = currentType.GetProperties(BindingFlags.Public|BindingFlags.Instance);
                foreach (PropertyInfo propertyInfo in propertyInfoArray ) {
                    // process all BuildAttribute attributes
                    BuildAttributeAttribute buildAttribute = (BuildAttributeAttribute) 
                        Attribute.GetCustomAttribute(propertyInfo, typeof(BuildAttributeAttribute));

                    if (buildAttribute != null) {
                        XmlNode attributeNode = elementNode.Attributes[buildAttribute.Name];

                        // check if its required
                        if (attributeNode == null && buildAttribute.Required) {
                            throw new BuildException(String.Format("'{0}' is a required attribute.", buildAttribute.Name), Location);
                        }

                        if (attributeNode != null) {
                            string attrValue = attributeNode.Value;
                            if (buildAttribute.ExpandProperties) {
                                // expand attribute properites
                                attrValue = Project.ExpandProperties(attrValue);
                            }

                            if (propertyInfo.CanWrite) {
                                // set the property value instead
                                MethodInfo info = propertyInfo.GetSetMethod();
                                object[] paramaters = new object[1];

                                // If the object is an emum
                                Type propertyType = propertyInfo.PropertyType;

                                if (propertyType.IsSubclassOf(Type.GetType("System.Enum"))) {
                                    try {
                                        paramaters[0] = Enum.Parse(propertyType, attrValue);
                                    } catch (Exception) {
                                        // catch type conversion exceptions here
                                        string message = "Invalid value \"" + attrValue + "\". Valid values for this attribute are: ";
                                        foreach (object value in Enum.GetValues(propertyType)) {
                                            message += value.ToString() + ", ";
                                        }
                                        // strip last ,
                                        message = message.Substring(0, message.Length - 2);
                                        throw new BuildException(message, Location);
                                    }
                                } else {
                                    paramaters[0] = Convert.ChangeType(attrValue, propertyInfo.PropertyType);
                                }
                                info.Invoke(this, paramaters);
                            }
                        }
                    }

                    // now do nested BuildElements
                    BuildElementAttribute buildElementAttribute = (BuildElementAttribute) 
                        Attribute.GetCustomAttribute(propertyInfo, typeof(BuildElementAttribute));

                    if (buildElementAttribute != null) {
                        // get value from xml node
                        XmlNode nestedElementNode = elementNode[buildElementAttribute.Name, elementNode.OwnerDocument.DocumentElement.NamespaceURI]; 
                        // check if its required
                        if (nestedElementNode == null && buildElementAttribute.Required) {
                            throw new BuildException(String.Format("'{0}' is a required element.", buildElementAttribute.Name), Location);
                        }
                        if (nestedElementNode != null) {
                            Element childElement = (Element)propertyInfo.GetValue(this, null);
                            childElement.Project = Project;
                            childElement.Initialize(nestedElementNode);
                        }
                        continue;
                    }
                }
                currentType = currentType.BaseType;
            }
        }

        /// <summary>Performs default initialization.</summary>
        /// <remarks>
        ///   <para>Derived classes that wish to add custom initialization should override <see cref="InitializeElement"/>.</para>
        /// </remarks>
        public void Initialize(XmlNode elementNode) {
            if (Project == null) {
                throw new InvalidOperationException("Element has invalid Project property.");
            }

            // Save position in buildfile for reporting useful error messages.
            try {
                _location = Project.LocationMap.GetLocation(elementNode);
            }
            catch(ArgumentException ae) {
                Log.WriteLineIf(Project.Verbose, ae.ToString());
                //ignore
            }

            // TODO: combine these two as they traverse the same data ...
            //ExpandProperties(elementNode);
            InitializeAttributes(elementNode);

            // Allow inherited classes a chance to do some custom initialization.
            InitializeElement(elementNode);
        }

        /// <summary>Allows derived classes to provide extra initialization and validation not covered by the base class.</summary>
        /// <param name="elementNode">The xml node of the element to use for initialization.</param>
        protected virtual void InitializeElement(XmlNode elementNode) {
        }
    }
}
