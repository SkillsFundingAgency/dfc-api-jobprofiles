# Digital First Careers - Job Profiles Api

This project provides a Job Profiles API for external use

Details of the Job Profiles application may be found here https://github.com/SkillsFundingAgency/dfc-app-jobprofiles

Details of the Composite UI application may be found here https://github.com/SkillsFundingAgency/dfc-composite-shell


## Getting Started

This is a self-contained Visual Studio 2019 solution containing a number of projects (web application, service and repository layers, with associated unit test and integration test projects).

### Installing

Clone the project and open the solution in Visual Studio 2019.

## List of dependencies

|Item	|Purpose|
|-------|-------|
|Azure Cosmos DB | Document storage |

## Local Config Files

Once you have cloned the public repo you need to rename the local.settings files by removing the -template part from the configuration file names listed below.

| Location | Repo Filename | Rename to |
|-------|-------|-------|
| DFC.Api.JobProfile | local.settings-template.json | local.settings.json |

## Configuring to run locally

The project contains a number of "local.settings-template.json" files which contain sample local.settings for the web app and the integration test projects. To use these files, rename them to "local.settings.json" and edit and replace the configuration item values with values suitable for your environment.

By default, the local.settings include a local Azure Cosmos Emulator configuration using the well known configuration values. These may be changed to suit your environment if you are not using the Azure Cosmos Emulator. In addition, Sitefinity configuration settings will need to be edited.

The settings also include the parameters required to call Azure Search, which are:

| Section | Parameter | Value |
|-------|-------|-------|
| JobProfileSearchIndexConfig | SearchIndex | search-index |
| JobProfileSearchIndexConfig | SearchServiceName | search-service-name |
| JobProfileSearchIndexConfig | AccessKey | search-index-access-key |

## Running locally

To run this product locally, you will need to configure the list of dependencies. Once configured and the configuration files updated, it should be F5 to run and debug locally. The application can be run using IIS Express or full IIS.

To run the project, start the JobProfileFunction Azure function app. On your local environment, swagger documentation is available at http://localhost:7071/api/swagger/ui

The Job Profile API function app has 3 endpoints:
- /job-profiles - fetches a summary list of job profiles stored in the main jobProfiles cosmos collection 
- /job-profiles/{canonicalName} - fetches the detail of a specific job profile
- /job-profiles/search/{searchTerm} - searches for matches across job profiles

## Deployments

This Job Profile Api function app will be deployed as an individual deployment for consumption by the Composite UI.

## Assets

CSS, JS, images and fonts used in this site can found in the following repository https://github.com/SkillsFundingAgency/dfc-digital-assets

## Built With

* Microsoft Visual Studio 2019
* .Net Core 2.2

## References

Please refer to https://github.com/SkillsFundingAgency/dfc-digital for additional instructions on configuring individual components like Sitefinity and Cosmos.
