# HttpClientInjector

HttpClientInjector is intended to provide a straightforward way to inject
HttpClients into classes, services or anything else that uses .Net Dependency Injection.

This library provides a fluent api for registering the instances you require.

## Status

This project uses SonarCloud for static analysis and vulnerability analysis.  
[![SonarQube Cloud](https://sonarcloud.io/images/project_badges/sonarcloud-light.svg)](https://sonarcloud.io/summary/new_code?id=daniel-buchanan_HttpClientInjector)  

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=daniel-buchanan_HttpClientInjector&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=daniel-buchanan_HttpClientInjector)  
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=daniel-buchanan_HttpClientInjector&metric=coverage)](https://sonarcloud.io/summary/new_code?id=daniel-buchanan_HttpClientInjector)  
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=daniel-buchanan_HttpClientInjector&metric=bugs)](https://sonarcloud.io/summary/new_code?id=daniel-buchanan_HttpClientInjector)  
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=daniel-buchanan_HttpClientInjector&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=daniel-buchanan_HttpClientInjector)  
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=daniel-buchanan_HttpClientInjector&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=daniel-buchanan_HttpClientInjector)  
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=daniel-buchanan_HttpClientInjector&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=daniel-buchanan_HttpClientInjector)  
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=daniel-buchanan_HttpClientInjector&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=daniel-buchanan_HttpClientInjector)  
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=daniel-buchanan_HttpClientInjector&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=daniel-buchanan_HttpClientInjector)  
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=daniel-buchanan_HttpClientInjector&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=daniel-buchanan_HttpClientInjector)  
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=daniel-buchanan_HttpClientInjector&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=daniel-buchanan_HttpClientInjector)  
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=daniel-buchanan_HttpClientInjector&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=daniel-buchanan_HttpClientInjector)


## Builder
This library provides a fluent builder api to allow for either immediate or Just In Time initialisation
of HttpClients as injected into your code.
