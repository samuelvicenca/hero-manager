CREATE TABLE Herois (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(120) NOT NULL,
    NomeHeroi NVARCHAR(120) NOT NULL,
    DataNascimento DATETIME2(7) NOT NULL,
    Altura FLOAT NOT NULL,
    Peso FLOAT NOT NULL,
    CONSTRAINT UQ_Herois_NomeHeroi UNIQUE (NomeHeroi)
);

CREATE TABLE Superpoderes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Superpoder NVARCHAR(50) NOT NULL,
    Descricao NVARCHAR(250) NOT NULL
);

CREATE TABLE HeroisSuperpoderes (
    HeroiId INT NOT NULL,
    SuperpoderId INT NOT NULL,
    PRIMARY KEY (HeroiId, SuperpoderId),
    CONSTRAINT FK_HeroisSuperpoderes_Herois FOREIGN KEY (HeroiId) REFERENCES Herois(Id),
    CONSTRAINT FK_HeroisSuperpoderes_Superpoderes FOREIGN KEY (SuperpoderId) REFERENCES Superpoderes(Id)
);
