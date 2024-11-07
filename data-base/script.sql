create database my_account_db; 

create table Public."User"
(
    "Id" uuid not null,
    "FirstName" varchar(100), 
    "LastName" varchar(100), 
    "CreationDate" timestamp, 
    "UserType" varchar(30), 
    "Email" varchar(100), 
    "IsActive" boolean, 
    "RegistrationMethod" varchar(50), 
    constraint pk_User primary key ("Id")
); 

create table Public."UserSecurity" 
(
    "Id" uuid not null,
    "UserId" uuid not null,
    "PasswordHash" varchar(255),
    "PasswordSalt" varchar(255),
    "LastPasswordChangeDate" timestamp,
    constraint fk_UserSecurity_User foreign key ("UserId") references Public."User"("Id") on delete cascade
);

create table Public."Account"
(
    "Id" uuid not null,
    "UserId" uuid not null, 
    "Description" varchar(300), 
    "CreationDate" timestamp, 
    "IsActive" boolean, 
    constraint pk_Account primary key ("Id"), 
    constraint fk_User foreign key ("UserId") references public."User"("Id")
);

create table Public."Sheet"
(
    "Id" uuid not null,
    "AccountId" uuid not null, 
    "Description" varchar(300) not null, 
    "CreationDate" timestamp, 
    "CashBalance" integer, 
    "CurrentAccountBalance" integer, 
    "Order" integer, 
    constraint pk_Sheet primary key ("Id"), 
    constraint fk_Account foreign key ("AccountId") references public."Account"("Id")
);

create table Public."Card"
(
    "Id" uuid not null,
    "SheetId" uuid not null, 
    "Title" varchar(100), 
    "Description" varchar(300), 
    "CreationDate" timestamp, 
    "Color" varchar(50), 
    constraint pk_Card primary key ("Id"), 
    constraint fk_Sheet foreign key ("SheetId") references public."Sheet"("Id")
);

create table Public."Vignette"
(
    "Id" uuid not null,
    "CardId" uuid not null, 
    "Description" varchar(300), 
    "Amount" integer, 
    "Color" varchar(50), 
    "Order" integer, 
    constraint pk_Bullet primary key ("Id"), 
    constraint fk_Card foreign key ("CardId") references public."Card"("Id")
);

/*
    select * from "User";
    select * from "UserSecurity"; 
    select * from "Account";
    select * from "Sheet";
    select * from "Card";
    select * from "Vignette";
*/