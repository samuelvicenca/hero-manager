-- ============================================
-- 1) Criar o banco de dados
-- ============================================
CREATE DATABASE HeroManagerDb;
GO

USE HeroManagerDb;
GO

-- ===============================
-- Tabela Herois
-- ===============================
CREATE TABLE Herois (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nome NVARCHAR(120) NOT NULL,
    NomeHeroi NVARCHAR(120) NOT NULL,
    DataNascimento DATETIME2 NOT NULL,
    Altura FLOAT NOT NULL,
    Peso FLOAT NOT NULL,
    CONSTRAINT UQ_Herois_NomeHeroi UNIQUE (NomeHeroi)
);
GO

-- ===============================
-- Tabela Superpoderes
-- ===============================
CREATE TABLE Superpoderes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Superpoder NVARCHAR(50) NOT NULL,
    Descricao NVARCHAR(250) NOT NULL
);
GO

-- ===============================
-- Tabela N:N HeroisSuperpoderes
-- ===============================
CREATE TABLE HeroisSuperpoderes (
    HeroiId INT NOT NULL,
    SuperpoderId INT NOT NULL,

    CONSTRAINT PK_HeroisSuperpoderes PRIMARY KEY (HeroiId, SuperpoderId),

    CONSTRAINT FK_HeroisSuperpoderes_Herois FOREIGN KEY (HeroiId)
        REFERENCES Herois(Id) ON DELETE CASCADE,

    CONSTRAINT FK_HeroisSuperpoderes_Superpoderes FOREIGN KEY (SuperpoderId)
        REFERENCES Superpoderes(Id) ON DELETE CASCADE
);
GO

-- Índices opcionais
CREATE INDEX IX_HeroisSuperpoderes_HeroiId ON HeroisSuperpoderes(HeroiId);
CREATE INDEX IX_HeroisSuperpoderes_SuperpoderId ON HeroisSuperpoderes(SuperpoderId);
GO


-- ===============================
-- Carga inicial super poder
-- ===============================
INSERT INTO Superpoderes (Superpoder, Descricao)
VALUES
('Super Força', 'Força extrema.'),
('Voo', 'Capacidade de voar.'),
('Invisibilidade', 'Ficar invisível.'),
('Velocidade', 'Mover-se muito rápido.'),
('Telecinese', 'Mover objetos com a mente.');
