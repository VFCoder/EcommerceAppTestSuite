# Automated E-commerce App Testing Framework

Welcome to the Automated E-commerce App Testing Framework repository. This project is intended to demonstrate an automated testing framework and test suite using .NET 7, C#, Page Object Model (POM), and NUnit. The framework includes UI tests, API tests, and database tests, all designed to verify the functionality of an open-source e-commerce application which I have installed on my local machine and published to azure cloud. This framework also utilizes fake data generation and test reporting. Note: This is not a complete project but just a sample of some approaches that I have used in past projects with my software companies.

## Table of Contents
- [Project Overview](#project-overview)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Setting up the E-commerce App and API Plugin](#setting-up-the-e-commerce-app-and-api-plugin)
  - [Running the Tests](#running-the-tests)
- [Test Framework Structure](#test-framework-structure)
- [Test Types](#test-types)
- [License](#license)

## Project Overview

This project tests an open-source e-commerce application. The application consists of the following components:

- Customer-side UI
- Admin panel
- API
- Database

I've set up two separate instances of the application, one on my local machine and another on Azure App Service. This allows others to access and run the tests against both instances, providing a comprehensive testing environment.

## Getting Started

To get started with this framework and run the tests, follow the instructions below.

### Prerequisites

Before you can run the tests, make sure you have the following prerequisites installed:

- [.NET 7 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- Visual Studio or any code editor of your choice

### Setting up the E-commerce App and API Plugin

To run the tests, you can access the cloud version of the app for UI testing located at [EcommerceTestingApp](https://ecommercetestingapp.azurewebsites.net/), and you also have the option to install the e-commerce app and the API plugin on your local machine. This approach provides greater control and flexibility for your testing needs, as well as the ability to run the database tests.

#### E-commerce App Setup

1. **Download the App**: Begin by downloading the e-commerce app from the official repository [nopCommerce](https://github.com/nopSolutions/nopCommerce) to your local machine.

2. **Database Setup**: Follow the database setup instructions provided in the nopCommerce app installation to configure and populate the database. You can find detailed guidance in their documentation for creating your local database environment.

#### API Plugin Setup

For API testing, you can use the API plugin designed for nopCommerce. Here's how to set it up:

1. **Download the API Plugin**: Obtain the API plugin from the official repository [API for nopCommerce](https://github.com/stepanbenes/api-for-nopcommerce).

2. **Integration**: Integrate the API plugin with your locally installed nopCommerce application by following the provided API plugin documentation and setup instructions.

By setting up the e-commerce app and the API plugin locally, you'll have full control over the testing environment, enabling you to run tests with custom configurations and data. This approach is particularly useful for database testing, where you can create and manage your own data scenarios for testing purposes.

Remember to configure the testing framework to use the local instances of the e-commerce app and the API plugin as part of your testing environment.

### Running the Tests

Clone this repository to your local machine:

```bash
git clone https://github.com/VFCoder/EcommerceAppTestSuite.git
```

Install the required NuGet packages by restoring the dependencies. This can be done in Visual Studio or via the command line:
```bash
dotnet restore
```
Now you can run the tests based on your requirements. The framework includes the following test types:

UI Tests:

Smoke Tests: Ensure the basic functionality of the UI.

Functional Tests: Test specific features of the UI.

Cross-Browser Tests: Test the UI on multiple browsers.

End-to-End Tests: Test complete user workflows.

API Tests: Verify the functionality of the API.

Database Tests: Ensure data integrity and consistency.

The specific steps to run these tests may vary, so please refer to the individual test sections in the framework for detailed instructions.

### Test Framework Structure
The test framework is structured according to the Page Object Model (POM) design pattern, promoting maintainability and readability of the tests.

### Test Types
NUnit: This framework uses NUnit for unit testing.

### License
This project is open source and available under the MIT License.

Thank you for checking out the Automated E-commerce App Testing Framework. If you have any questions or need assistance running the tests, please don't hesitate to reach out. Happy testing!
