string NuGetVersionV2 = "";
string SolutionFileName = "src/StoneAssemblies.Keycloak.sln";

string[] DockerFiles = System.Array.Empty<string>();

string[] OutputImages = System.Array.Empty<string>();

string[] ComponentProjects  = new [] {
	"./src/StoneAssemblies.Keycloak/StoneAssemblies.Keycloak.csproj"
};

string TestProject = "src/StoneAssemblies.Keycloak.Tests/StoneAssemblies.Keycloak.Tests.csproj";

string SonarProjectKey = "stoneassemblies_Keycloak";
string SonarOrganization = "stoneassemblies";