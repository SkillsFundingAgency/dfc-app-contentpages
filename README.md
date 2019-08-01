# dfc-app-help

This project provides a Help App for use in the Composite UI (Shell application) to dynamically output markup from Help data sources.

This Help app runs in two flavours:

* Help documents
* Draft Help documents

The Help app also provisions the following for consumption by the Composit UI:

* Sitemap.xml for all Help documents
* Robots.txt

## Getting Started

This is a self-contained Visual Studio 2019 solution containing a number of projects (web application, service and repository layes, with associated unit test and integration test projects).

### Prerequisites

Microsoft Visual Studio 2019 with .Net core 2.2
Azure Cosmos DB environmnet - either in Azure or a local emulator
Sitefinity ???????

### Installing

Clone the project and open the solution in Visual Studio 2019.

## Configuring to run locally

The project contains a number of "appsettings-template.json" files which contain sample appsettings for the web app and the integration tests projects. To use these files, rename them to "appsettings.json" and edit and replace the configuration item values with values suitable for your environment.

By default, the appsettings include a local CosmosDB configuration using the well known configuration values. These may be changed to suit your environment if you are nto using the Azure Cosmos Emulator.

## Running locally

To run the project, start the web application.

Once running, browse to the main entrypoint which is the "https://localhost:44329/pages". This will list all of the Help pages available and from here, you can navigate to the individual help pages.

The Help app is designed to be run from within the Composite UI, therefore running the Help app outside of the Composite UI will only show simple views of the data.

## Deployments

This Help app will be deployed as an individual deploymnet for consumption by the Composite UI.

## Built With

* Microsoft Visual Studio 2019
* .Net Core 2.2

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
