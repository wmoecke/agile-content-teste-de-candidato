using CandidateTesting.WernerMoecke.Converter.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace CandidateTesting.WernerMoecke.UnitTests
{
    /// <summary>
    /// Class responsible of performing unit tests for<br>
    /// CandidateTesting.WernerMoecke.Converter class.
    /// </summary>
    public class ConverterTests : IDisposable
    {
        private const string _inputFile = "../../../input-01.txt";
        private const string _outputFile = "../../../log.txt";

        /// <summary>
        /// SetUp
        /// </summary>
        public ConverterTests()
        {
        }

        /// <summary>
        /// TearDown
        /// </summary>
        public void Dispose()
        {
            File.Delete(Path.GetFullPath(_outputFile));
        }

        /// <summary>
        /// Validates proper creation of a 'log.txt' file.
        /// </summary>
        [Fact]
        public void Convert_WithParameter_ShouldCreateFile()
        {
            // Arrange
            var converter = new Converter.Converter();
            var sourceUri = new Uri(Path.GetFullPath(_inputFile)).AbsolutePath;
            var outputPath = Path.GetFullPath(_outputFile);

            // Act
            converter.Convert(sourceUri, outputPath);

            // Assert
            File.Exists(outputPath).Should().BeTrue();
        }

        /// <summary>
        /// Validates if calls to ConvertFile() method are being made from within the Convert() method.
        /// </summary>
        [Fact]
        public void Convert_WithParameter_ShouldInvokeConvertFile()
        {
            // Arrange
            var mock = new Mock<Converter.Converter>() { CallBase = true };
            mock.Setup(x => x.ConvertFile(It.IsAny<Uri>())).Verifiable();
            var converter = mock.Object;
            var sourceUri = new Uri(Path.GetFullPath(_inputFile)).AbsolutePath;
            var outputPath = Path.GetFullPath(_outputFile);

            // Act
            converter.Convert(sourceUri, outputPath);

            // Assert
            mock.Verify(x => x.ConvertFile(It.IsAny<Uri>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Validates output of the ConvertFile() method.
        /// </summary>
        [Fact]
        public void ConvertFile_WithParameter_ShouldReturnCorrectOutput()
        {
            // Arrange
            var converter = new Converter.Converter();
            Uri fileUri = new Uri(Path.GetFullPath(_inputFile));
            string expected = "\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT|\"MINHA CDN\" POST 200 /myImages 319 101 MISS|\"MINHA CDN\" GET 404 /not-found 143 199 MISS|\"MINHA CDN\" GET 200 /robots.txt 245 312 INVALIDATE|";

            // Act
            var result = converter.ConvertFile(fileUri);

            // Assert
            result.Should().Be(expected);
        }

        /// <summary>
        /// Validates if calls to ConvertLine() method are being made from within the ConvertFile() method.
        /// </summary>
        [Fact]
        public void ConvertFile_WithParameter_ShouldInvokeConvertLine()
        {
            // Arrange
            var mock = new Mock<Converter.Converter>() { CallBase = true };
            mock.Setup(x => x.ConvertLine(It.IsAny<string>())).Verifiable();
            var converter = mock.Object;
            Uri fileUri = new Uri(Path.GetFullPath(_inputFile));

            // Act
            _ = converter.ConvertFile(fileUri);

            // Assert
            mock.Verify(x => x.ConvertLine(It.IsAny<string>()), Times.AtLeastOnce);
        }

        /// <summary>
        /// Validates output of the ConvertLine() method.
        /// </summary>
        [Fact]
        public void ConvertLine_WithParameter_ShouldReturnCorrectOutput()
        {
            // Arrange
            var converter = new Converter.Converter();
            string line = "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2";
            string expected = "\"MINHA CDN\" GET 200 /robots.txt 100 312 HIT|";

            // Act
            var result = converter.ConvertLine(line);

            // Assert
            result.Should().Be(expected);
        }
    }
}