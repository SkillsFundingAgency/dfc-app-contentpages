﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFC.App.Help.Data;
using DFC.App.Help.Data.Contracts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DFC.App.Help.IntegrationTests.ServiceTests.HelpPageServiceTests
{
    [TestFixture]
    public class HelpPageServiceGetAllTests : BaseHelpPageServiceTests
    {
        [Test]
        [Category("HelpPageService.GetAllList")]
        public async Task HelpPageServiceGetAllListReturnsSuccessWhenHelpPagesExist()
        {
            // arrange
            const string name = ValidNameValue + "_GetList";
            var helpPageModels = new List<HelpPageModel>() {
                new HelpPageModel() {
                    CanonicalName = name + "_1",
                    DocumentId=Guid.NewGuid()
                },
                new HelpPageModel()
                {
                    CanonicalName = name + "_2",
                    DocumentId=Guid.NewGuid()
                }
            };
            var helpPageService = _serviceProvider.GetService<IHelpPageService>();

            helpPageModels.ForEach(async f => _ = await helpPageService.CreateAsync(f));

            // act
            var results = await helpPageService.GetAllAsync();
            var resultsList = results.ToList();

            // assert
            resultsList.Should().NotBeNull();
            resultsList.Count.Should().BeGreaterOrEqualTo(helpPageModels.Count);
            resultsList.Should().Contain(x => helpPageModels.Any(y => y.DocumentId == x.DocumentId));
            resultsList[0].CanonicalName.Should().NotBeNull();
            resultsList[1].CanonicalName.Should().NotBeNull();
        }
    }
}
