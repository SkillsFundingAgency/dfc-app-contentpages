# Digital First Careers – Help app

## Introduction

This project provides a Help App for use in the Composite UI (Shell application) to dynamically output markup from Help data sources.

Details of the Composite UI application may be found here https://github.com/SkillsFundingAgency/dfc-composite-shell

This Help app runs in two flavours:

* Help documents
* Draft Help documents

The Help app also provisions the following for consumption by the Composite UI:

* Sitemap.xml for all Help documents
* Robots.txt

## Getting Started

This is a self-contained Visual Studio 2019 solution containing a number of projects (web application, service and repository layes, with associated unit test and integration test projects).

### Installing

Clone the project and open the solution in Visual Studio 2019.

## List of dependencies

|Item	|Purpose|
|-------|-------|
|Sitefinity |Content management system |
|Azure Cosmos DB | Document storage |

## Local Config Files

Once you have cloned the public repo you need to remove the -template part from the configuration file names listed below.

| Location | Repo Filename | Rename to |
|-------|-------|-------|
| DFC.App.Help.IntegrationTests | appsettings-template.json | appsettings.json |
| DFC.App.Help | appsettings-template.json | appsettings.json |

## Configuring to run locally

The project contains a number of "appsettings-template.json" files which contain sample appsettings for the web app and the integration test projects. To use these files, rename them to "appsettings.json" and edit and replace the configuration item values with values suitable for your environment.

By default, the appsettings include a local Azure Cosmos Emulator configuration using the well known configuration values. These may be changed to suit your environment if you are not using the Azure Cosmos Emulator. In addition, Sitefinity configuration settings will need to be edited.

|File                                       |Setting                        |Example value                      |
|------------------------------------------:|------------------------------:|----------------------------------:|
| appsettings.json     | SitefinityApi.AuthTokenEndpoint      |< your domain authentication endpoint >  |
| appsettings.json     | SitefinityApi.SitefinityApiUrlBase     |http://< your domain api base endpoint >  |
| appsettings.json     | SitefinityApi.SitefinityApiDataEndpoint |< your domain api data endpoint >  |
| appsettings.json     | SitefinityApi.ClientId           | < can be obtained from sitefinity  >|
| appsettings.json     | SitefinityApi.ClientSecret       | < generate it with sitefinity >     |
| appsettings.json     | SitefinityApi.Username           | < sitefinity username >             |
| appsettings.json     | SitefinityApi.Password           | < sitefinity password >             |
| appsettings.json     | SitefinityApi.Scopes             | < authentication protocol> OpenId   |

## Running locally

To run this product locally, you will need to configure the list of dependencies, once configured and the configuration files updated, it should be F5 to run and debug locally. The application can be run using IIS Express or full IIS.

To run the project, start the web application. Once running, browse to the main entrypoint which is the "https://localhost:44329/pages". This will list all of the Help pages available and from here, you can navigate to the individual help pages.

The Help app is designed to be run from within the Composite UI, therefore running the Help app outside of the Composite UI will only show simple views of the data.

## Deployments

This Help app will be deployed as an individual deployment for consumption by the Composite UI.

## Assets

CSS, JS, images and fonts used in this site can found in the following repository https://github.com/SkillsFundingAgency/dfc-digital-assets

## Built With

* Microsoft Visual Studio 2019
* .Net Core 2.2

## References

Please refer to https://github.com/SkillsFundingAgency/dfc-digital for additional instructions on configuring individual components like Sitefinity and Cosmos.
