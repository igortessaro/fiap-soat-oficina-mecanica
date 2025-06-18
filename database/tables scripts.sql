CREATE TABLE "Clients" (
  "Id" Guid PRIMARY KEY,
  "Document" varchar,
  "FullName" varchar,
  "Phone" varchar,
  "Email" varchar,
  "Address" varchar,
  "CreatedAt" timestamp,
  "UpdatedAt" timestamp
);

CREATE TABLE "Vehicles" (
  "Id" Guid PRIMARY KEY,
  "LicensePlate" varchar,
  "Year" int,
  "Brand" varchar,
  "Model" varchar,
  "CreatedAt" timestamp,
  "UpdatedAt" timestamp,
  "ClientId" Guid
);

CREATE TABLE "AvailableServices" (
  "Id" Guid PRIMARY KEY,
  "Name" varchar,
  "Price" decimal,
  "CreatedAt" timestamp,
  "UpdatedAt" timestamp
);

CREATE TABLE "Supplies" (
  "Id" Guid PRIMARY KEY,
  "Name" varchar,
  "Quantity" decimal,
  "Price" decimal,
  "CreatedAt" timestamp,
  "UpdatedAt" timestamp
);

CREATE TABLE "AvailableServices_Supplies" (
  "Id" Guid PRIMARY KEY,
  "AvailableServiceId" Guid,
  "SupplyId" Guid,
  "CreatedAt" timestamp,
  "UpdatedAt" timestamp
);

CREATE TABLE "ServiceOrders" (
  "Id" Guid PRIMARY KEY,
  "CreatedAt" timestamp,
  "UpdatedAt" timestamp,
  "Status" varchar,
  "ClientId" Guid,
  "VehicleId" Guid
);

CREATE TABLE "ServiceOrders_AvailableServices" (
  "Id" Guid PRIMARY KEY,
  "ServiceOrderId" Guid,
  "AvailableServiceId" Guid,
  "CreatedAt" timestamp,
  "UpdatedAt" timestamp
);

ALTER TABLE "Vehicles" ADD FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id");

ALTER TABLE "ServiceOrders" ADD FOREIGN KEY ("ClientId") REFERENCES "Clients" ("Id");

ALTER TABLE "ServiceOrders" ADD FOREIGN KEY ("VehicleId") REFERENCES "Vehicles" ("Id");

ALTER TABLE "ServiceOrders_AvailableServices" ADD FOREIGN KEY ("ServiceOrderId") REFERENCES "ServiceOrders" ("Id");

ALTER TABLE "ServiceOrders_AvailableServices" ADD FOREIGN KEY ("AvailableServiceId") REFERENCES "AvailableServices" ("Id");

ALTER TABLE "AvailableServices_Supplies" ADD FOREIGN KEY ("AvailableServiceId") REFERENCES "AvailableServices" ("Id");

ALTER TABLE "AvailableServices_Supplies" ADD FOREIGN KEY ("SupplyId") REFERENCES "Supplies" ("Id");
