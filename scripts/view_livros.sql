CREATE VIEW v_livros AS
SELECT l.CodL, l.Titulo, l.Editora, l.Edicao, l.AnoPublicacao, l.Preco, au.CodAu, au.Nome, ass.CodAs, ass.Descricao FROM Livro l
INNER JOIN Livro_Assunto las On las.Livro_CodL = l.CodL 
INNER JOIN Livro_Autor lau On lau.Livro_CodL = l.CodL 
INNER JOIN Autor au ON au.CodAu  = lau.Autor_CodAu 
INNER JOIN Assunto ass ON ass.CodAs  = las.Assunto_CodAs 