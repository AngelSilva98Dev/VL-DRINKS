use VLDRINKS

insert into USUARIO (Nombres, Apellidos, Correo, Clave) values ('nombreprueba', 'apellidoprueba','prueba@gmail.com','5ac0852e770506dcd80f1a36d20ba7878bf82244b836d9324593bd14bc56dcb5')

select * from USUARIO

select * from CATEGORIA

insert into CATEGORIA(Descripcion) values ('Cervezas'), ('Espirituosas'), ('Gaseosas'),('Vinos')

select * from MARCA

insert into MARCA (Descripcion) values ('Branca'), ('Heineken'), ('Quilmes'),('Andes')

create table PROVINCIA(
IdProvincia varchar (2) not null,
Descrípcion varchar (45) not null
)

create table DEPARTAMENTO(
IdDepartamento varchar (4) not null,
Descripcion varchar (45) not null,
IdProvincia varchar (2) not null
)

create table BARRIO(
IdBarrio varchar (6) not null,
Descripcion varchar (45) not null,
IdProvincia varchar (4) not null,
IdDepartamento varchar (2) not null
)

ALTER TABLE BARRIO
ALTER COLUMN IdDepartamento VARCHAR(4);

insert into PROVINCIA (IdProvincia, Descrípcion) values ('01','Córdoba'), ('02','Buenos Aires')

SELECT * from PROVINCIA

EXEC sp_rename 'PROVINCIA.Descrípcion', 'Descripcion', 'COLUMN';

insert into DEPARTAMENTO (IdDepartamento, Descripcion, Idprovincia) values ('0101','Capital','01'), ('0102','Punilla','01')

select * from DEPARTAMENTO

select * from BARRIO

insert into BARRIO (IdBarrio, Descripcion, IdProvincia, IdDepartamento) values ('010101','Empalme','01','0101'), ('010102','San Vicente','01','0101')

