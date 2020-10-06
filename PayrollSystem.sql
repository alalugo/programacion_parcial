USE [master]
GO

CREATE DATABASE PayrollSystem
GO

USE PayrollSystem
GO

CREATE TABLE Departamentos
(
	Id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	Nombre VARCHAR(200) NOT NULL,
	UbicacionFisica VARCHAR(200) NOT NULL
)

CREATE TABLE Puestos
(
	Id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	Nombre VARCHAR(200) NOT NULL,
	NivelRiesgo VARCHAR(100) NOT NULL,
	NivelMinimoSalario MONEY,
	NivelMaximoSalario MONEY
)

CREATE TABLE Empleados
(
	Id	INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	Cedula VARCHAR(13) NOT NULL,
	Nombre VARCHAR(200) NOT NULL,
	IdDepartamento INT NOT NULL FOREIGN KEY REFERENCES Departamentos(Id),
	IdPuesto INT NOT NULL FOREIGN KEY REFERENCES Puestos(Id),
	SalarioMensual MONEY DEFAULT 0.00
)


CREATE TABLE TipoIngreso
(
	Id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	Nombre VARCHAR(200) NOT NULL,
	DependeSalario VARCHAR(200) NOT NULL,
	Estado VARCHAR(100)
)

CREATE TABLE TiposDeducciones
(
	Id INT PRIMARY KEY NOT NULL IDENTITY(1,1),
	Nombre VARCHAR(200) NOT NULL,
	DependeSalario VARCHAR(200) NOT NULL,
	Estado VARCHAR(100)
)