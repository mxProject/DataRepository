
# Repository pattern library

## Overview

`mxProject.Data.Repository.Abstractions` package provides abstracted interfaces and classes for implementing the repository pattern. This library helps simplify the implementation of the data access layer and create testable code.

## Features

* Provides standard interfaces for the repository pattern
* Simplifies the implementation of the data access layer
* Supports both implicit and explicit transactions using TransactionScope
* Does not depend on ADO\.NET

## Installation

To install this library, use the NuGet package manager.

## Usage

* For supported repository interface types, see [Repogitory Interfaces](./document/RepogitoryInterfaces.md) .

* `mxProject.Data.DbRepository` package provides repositories for storing data in a database. see [Implementing database repositories](./document/DatabaseRepository.md) .

* `mxProject.Data.FileRepository` package and `mxProject.Data.FileRepository.TxFileManager` package provide a repository for storing data in files using the file system. see [Implementing filesystem repositories](./document/FileRepository.md) .

* `mxProject.Data.Repository.Caching` package adds caching capabilities to repositories. see [Caching](./document/Caching.md) .

## License

This project is licensed under the MIT License.
