create database my_account_db; 

create table Public."User"
(
    "Id" uuid not null,
    "FirstName" varchar(100), 
    "LastName" varchar(100), 
    "CreationDate" timestamp, 
    "UserType" varchar(30), 
    "Email" varchar(100), 
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

create table public."UserAccessLog"
(
  "Id" uuid not null default gen_random_uuid(),
  "UserId" uuid null,
  "OccurredAt" timestamptz not null default now(),  -- La fecha y hora exacta en que ocurrió el evento (login, logout, fallo, etc.).
  "EventType" varchar(40) not null,                 -- LOGIN_SUCCESS, LOGIN_FAILED, LOGOUT, TOKEN_REFRESHED, SESSION_EXPIRED
  "Success" boolean not null,
  "FailureReason" varchar(120) null,                -- INVALID_PASSWORD, USER_NOT_FOUND, ACCOUNT_LOCKED, TOO_MANY_ATTEMPTS, SESSION_EXPIRED, TOKEN_EXPIRED, TOKEN_INVALID
  "IpAddress" text null,
  "UserAgent" text null,                            -- Cadena que envía el navegador o app, por ejemplo: "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7)"
  "AuthProvider" varchar(50) null,                  -- Google, Apple, Manual
  constraint "FK_UserAccessLog_User" foreign key ("UserId") references public."User"("Id") on delete set null
);

create table Public."Account"
(
    "Id" uuid not null,
    "UserId" uuid not null, 
    "Description" varchar(300), 
    "CreationDate" timestamp, 
    "Order" integer, 
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
    "Order" integer, 
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