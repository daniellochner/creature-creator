using UnityEditor;

namespace IronSource.Editor
{
	public interface IAdapterSettings
	{
		void updateProject(BuildTarget buildTarget, string projectPath);
		void updateProjectPlist(BuildTarget buildTarget, string plistPath);
	}
}