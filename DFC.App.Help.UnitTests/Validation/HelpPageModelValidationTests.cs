using DFC.App.Help.Common;
using DFC.App.Help.Controllers;
using DFC.App.Help.Models.Cosmos;
using DFC.App.Help.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DFC.App.Help.UnitTests.Validation
{
    [TestFixture]
    public class HelpPageModelValidationTests
    {
        private PagesController _controller;
        private Mock<IHelpPageService> _helpPageService;
        private Mock<ILogger<PagesController>> _logger;

        [SetUp]
        public void SetUp()
        {
            _helpPageService = new Mock<IHelpPageService>();
            _logger = new Mock<ILogger<PagesController>>();

            _controller = new PagesController(_helpPageService.Object, _logger.Object);
        }

        [TestCase(null)]
        [TestCase(Constants.GuidEmpty)]
        public void CanCheck_IfDocumentIdIsInvalid(Guid documentId)
        {
            var model = CreateModel(documentId, "canonicalname1", "content1", new List<string>());

            var vr = Validate(model);

            vr.Should().NotBeEmpty();
            vr.Should().Contain(x => x.ErrorMessage == string.Format(ValidationMessage.FieldEmptyGuid, nameof(model.DocumentId)));
            vr.Should().HaveCount(1);
        }

        [TestCase("abcdefghijklmnopqrstuvwxyz")]
        [TestCase("0123456789")]
        [TestCase("abc")]
        [TestCase("xyz123")]
        [TestCase("abc_def")]
        [TestCase("abc-def")]
        public void CanCheck_IfCanonicalNameIsValid(string canonicalName)
        {
            var model = CreateModel(Guid.NewGuid(), canonicalName, "content", new List<string>());

            var vr = Validate(model);

            vr.Should().BeEmpty();
        }

        [TestCase("abcdefghijklmnopqrstuvwxyz")]
        [TestCase("0123456789")]
        [TestCase("abc")]
        [TestCase("xyz123")]
        [TestCase("abc_def")]
        [TestCase("abc-def")]
        public void CanCheck_IfAlternativeNameIsValid(string alternativeName)
        {
            var model = CreateModel(Guid.NewGuid(), "canonicalname1", "content1", new List<string>() { alternativeName });

            var vr = Validate(model);

            vr.Should().BeEmpty();
        }

        private HelpPageModel CreateModel(Guid documentId, string canonicalName, string content, List<string> alternativeNames)
        {
            var model = new HelpPageModel();

            model.DocumentId = documentId;
            model.CanonicalName = canonicalName;
            model.Content = content;
            model.AlternativeNames = alternativeNames.ToArray();

            return model;
        }

        private List<ValidationResult> Validate(HelpPageModel model)
        {
            var vr = new List<ValidationResult>();
            var vc = new ValidationContext(model);
            Validator.TryValidateObject(model, vc, vr, true);

            return vr;
        }
    }
}
