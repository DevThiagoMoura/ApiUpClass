-- ApiUpClass - script de banco MySQL
-- Use em uma base local de desenvolvimento.
-- Atencao: o bloco DROP remove as tabelas existentes.

CREATE DATABASE IF NOT EXISTS upclass
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE upclass;

SET FOREIGN_KEY_CHECKS = 0;

DROP TABLE IF EXISTS cursos_tags;
DROP TABLE IF EXISTS avaliacoes;
DROP TABLE IF EXISTS pagamentos;
DROP TABLE IF EXISTS matriculas;
DROP TABLE IF EXISTS aulas;
DROP TABLE IF EXISTS modulos;
DROP TABLE IF EXISTS cursos;
DROP TABLE IF EXISTS Tags;
DROP TABLE IF EXISTS categorias;
DROP TABLE IF EXISTS usuarios;

SET FOREIGN_KEY_CHECKS = 1;

CREATE TABLE usuarios (
  id INT AUTO_INCREMENT PRIMARY KEY,
  nome VARCHAR(150) NOT NULL,
  email VARCHAR(150) NOT NULL UNIQUE,
  senha_hash VARCHAR(255) NOT NULL,
  papel VARCHAR(20) NOT NULL DEFAULT 'aluno',
  criado_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT chk_usuarios_papel CHECK (papel IN ('aluno', 'instrutor', 'administrador'))
);

CREATE TABLE categorias (
  id INT AUTO_INCREMENT PRIMARY KEY,
  nome VARCHAR(100) NOT NULL,
  descricao TEXT NULL
);

CREATE TABLE cursos (
  id INT AUTO_INCREMENT PRIMARY KEY,
  titulo VARCHAR(200) NOT NULL,
  descricao TEXT NULL,
  preco DECIMAL(10,2) NOT NULL DEFAULT 0.00,
  ativo BOOLEAN NOT NULL DEFAULT TRUE,
  criado_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  categoria_id INT NOT NULL,
  CONSTRAINT fk_cursos_categorias
    FOREIGN KEY (categoria_id) REFERENCES categorias(id)
);

CREATE TABLE modulos (
  id INT AUTO_INCREMENT PRIMARY KEY,
  titulo VARCHAR(200) NOT NULL,
  ordem INT NOT NULL,
  curso_id INT NOT NULL,
  CONSTRAINT fk_modulos_cursos
    FOREIGN KEY (curso_id) REFERENCES cursos(id)
);

CREATE TABLE aulas (
  id INT AUTO_INCREMENT PRIMARY KEY,
  titulo VARCHAR(200) NOT NULL,
  modulo_id INT NOT NULL,
  duracao INT NULL,
  url_video VARCHAR(255) NULL,
  CONSTRAINT fk_aulas_modulos
    FOREIGN KEY (modulo_id) REFERENCES modulos(id)
);

CREATE TABLE Tags (
  id INT AUTO_INCREMENT PRIMARY KEY,
  nome VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE cursos_tags (
  curso_id INT NOT NULL,
  tag_id INT NOT NULL,
  PRIMARY KEY (curso_id, tag_id),
  CONSTRAINT fk_cursos_tags_cursos
    FOREIGN KEY (curso_id) REFERENCES cursos(id),
  CONSTRAINT fk_cursos_tags_tags
    FOREIGN KEY (tag_id) REFERENCES Tags(id)
);

CREATE TABLE matriculas (
  id INT AUTO_INCREMENT PRIMARY KEY,
  usuario_id INT NOT NULL,
  curso_id INT NOT NULL,
  data_matricula DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  status VARCHAR(20) NOT NULL DEFAULT 'ativo',
  CONSTRAINT fk_matriculas_usuarios
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id),
  CONSTRAINT fk_matriculas_cursos
    FOREIGN KEY (curso_id) REFERENCES cursos(id),
  CONSTRAINT uq_matriculas_usuario_curso UNIQUE (usuario_id, curso_id)
);

CREATE TABLE pagamentos (
  id INT AUTO_INCREMENT PRIMARY KEY,
  usuario_id INT NOT NULL,
  curso_id INT NOT NULL,
  valor DECIMAL(10,2) NOT NULL,
  status VARCHAR(20) NOT NULL DEFAULT 'pendente',
  criado_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_pagamentos_usuarios
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id),
  CONSTRAINT fk_pagamentos_cursos
    FOREIGN KEY (curso_id) REFERENCES cursos(id)
);

CREATE TABLE avaliacoes (
  id INT AUTO_INCREMENT PRIMARY KEY,
  usuario_id INT NOT NULL,
  curso_id INT NOT NULL,
  nota DECIMAL(2,1) NOT NULL,
  comentario TEXT NULL,
  criado_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  CONSTRAINT fk_avaliacoes_usuarios
    FOREIGN KEY (usuario_id) REFERENCES usuarios(id),
  CONSTRAINT fk_avaliacoes_cursos
    FOREIGN KEY (curso_id) REFERENCES cursos(id),
  CONSTRAINT uq_avaliacoes_usuario_curso UNIQUE (usuario_id, curso_id),
  CONSTRAINT chk_avaliacoes_nota CHECK (nota >= 1 AND nota <= 5)
);

INSERT INTO categorias (id, nome, descricao) VALUES
  (1, 'Programacao', 'Cursos da area de desenvolvimento'),
  (2, 'Banco de Dados', 'Cursos voltados para SQL e modelagem'),
  (3, 'Carreira', 'Cursos de organizacao e produtividade');

INSERT INTO cursos (id, titulo, descricao, preco, ativo, categoria_id) VALUES
  (1, 'C# com ASP.NET Core', 'Curso introdutorio para desenvolvimento de APIs', 99.90, TRUE, 1),
  (2, 'Modelagem de Banco de Dados', 'Curso sobre DER, modelo logico e SQL', 79.90, TRUE, 2),
  (3, 'Git e GitHub para Projetos', 'Curso pratico para versionamento em equipe', 49.90, TRUE, 3);

INSERT INTO modulos (id, titulo, ordem, curso_id) VALUES
  (1, 'Fundamentos de C#', 1, 1),
  (2, 'Criando APIs com ASP.NET Core', 2, 1),
  (3, 'Modelagem Relacional', 1, 2),
  (4, 'Fluxo de versionamento', 1, 3);

INSERT INTO aulas (id, titulo, modulo_id, duracao, url_video) VALUES
  (1, 'Introducao ao C#', 1, 25, 'https://video.local/csharp-introducao'),
  (2, 'Controllers e Services', 2, 35, 'https://video.local/controllers-services'),
  (3, 'Entidades e Relacionamentos', 3, 30, 'https://video.local/der'),
  (4, 'Branches e Commits', 4, 20, 'https://video.local/git-branches');

INSERT INTO Tags (id, nome) VALUES
  (1, 'backend'),
  (2, 'csharp'),
  (3, 'sql'),
  (4, 'git');

INSERT INTO cursos_tags (curso_id, tag_id) VALUES
  (1, 1),
  (1, 2),
  (2, 3),
  (3, 4);

-- Usuarios devem ser criados pela propria API para gerar senha_hash valido.
-- Use POST /usuarios no Swagger para criar aluno e instrutor.
