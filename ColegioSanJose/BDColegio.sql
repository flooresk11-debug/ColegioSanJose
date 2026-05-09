CREATE DATABASE IF NOT EXISTS ColegioSanJose;
USE ColegioSanJose;

CREATE TABLE Alumno (
    AlumnoId INT NOT NULL AUTO_INCREMENT,
    Nombre VARCHAR(100) NOT NULL,
    Apellido VARCHAR(100) NOT NULL,
    FechaNacimiento DATE NULL,
    Grado VARCHAR(50) NOT NULL,
    PRIMARY KEY (AlumnoId)
);

CREATE TABLE Materia (
    MateriaId INT NOT NULL AUTO_INCREMENT,
    NombreMateria VARCHAR(150) NOT NULL,
    Docente VARCHAR(150) NOT NULL,
    PRIMARY KEY (MateriaId)
);

CREATE TABLE Expediente (
    ExpedienteId INT NOT NULL AUTO_INCREMENT,
    AlumnoId INT NOT NULL,
    MateriaId INT NOT NULL,
    NotaFinal DECIMAL(5,2) NULL,
    Observaciones VARCHAR(500) NULL,
    PRIMARY KEY (ExpedienteId),
    FOREIGN KEY (AlumnoId) REFERENCES Alumno(AlumnoId) ON DELETE CASCADE,
    FOREIGN KEY (MateriaId) REFERENCES Materia(MateriaId) ON DELETE CASCADE
);