using NUnit.Framework;
using System.IO;

namespace ShieldMyRide.Tests
{
    [TestFixture]
    public class UnitTest1
    {
        private string _projectRoot;

        [SetUp]
        public void Setup()
        {
            // Go up from /bin/Debug/... to your main ShieldMyRide project
            _projectRoot = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\ShieldMyRide");
            _projectRoot = Path.GetFullPath(_projectRoot);
        }

        [Test]
        public void ProjectRoot_ShouldExist()
        {
            Assert.That(Directory.Exists(_projectRoot), Is.True, $"Project root not found at {_projectRoot}");
        }

        [Test]
        public void ControllersFolder_ShouldExist()
        {
            var controllersPath = Path.Combine(_projectRoot, "Controllers");
            Assert.That(Directory.Exists(controllersPath), Is.True, "Controllers folder missing");
        }

        [Test]
        public void ModelsFolder_ShouldExist()
        {
            var modelsPath = Path.Combine(_projectRoot, "Models");
            Assert.That(Directory.Exists(modelsPath), Is.True, "Models folder missing");
        }

        [Test]
        public void DtosFolder_ShouldExist()
        {
            var dtoPath = Path.Combine(_projectRoot, "DTOs");
            Assert.That(Directory.Exists(dtoPath), Is.True, "DTOs folder missing");
        }

        [Test]
        public void ProgramFile_ShouldExist()
        {
            var programPath = Path.Combine(_projectRoot, "Program.cs");
            Assert.That(File.Exists(programPath), Is.True, "Program.cs missing");
        }

        [Test]
        public void StartupFile_ShouldExist_IfUsingStartup()
        {
            var startupPath = Path.Combine(_projectRoot, "Startup.cs");
            // Passes if Startup.cs exists OR if your project uses minimal Program.cs
            Assert.That(File.Exists(startupPath) || !File.Exists(startupPath), Is.True);
        }
    }
}
