<%@ WebService Language="C#" Class="BuildServer" %>

using System.Web.Services;
using NAnt.BuildServer;

[WebService(Namespace="http://nant.sf.net/")]
public class BuildServer {
	[WebMethod]
	public void BuildProject(int projectId, string reason) {
		BuildQueue q = BuildQueue.GetBuildQueue();
		q.AddBuild(new Build(projectId, reason));
	}
}
