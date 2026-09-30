CREATE TABLE "Brands" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Brands" PRIMARY KEY AUTOINCREMENT,
    "ImgUrl" TEXT NOT NULL,
    "Name" TEXT NOT NULL
);


CREATE TABLE "Categories" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Categories" PRIMARY KEY AUTOINCREMENT,
    "FiltersJson" TEXT NOT NULL,
    "Icon" TEXT NOT NULL,
    "Name" TEXT NOT NULL
);


CREATE TABLE "Owners" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Owners" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL
);


CREATE TABLE "Models" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Models" PRIMARY KEY AUTOINCREMENT,
    "BrandId" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    CONSTRAINT "FK_Models_Brands_BrandId" FOREIGN KEY ("BrandId") REFERENCES "Brands" ("Id") ON DELETE CASCADE
);


CREATE TABLE "Cars" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Cars" PRIMARY KEY AUTOINCREMENT,
    "BrandId" INTEGER NOT NULL,
    "ModelId" INTEGER NOT NULL,
    "NumberPlate" TEXT NOT NULL,
    "Vin" TEXT NOT NULL,
    "Weight" REAL NOT NULL,
    "Year" INTEGER NOT NULL,
    CONSTRAINT "FK_Cars_Brands_BrandId" FOREIGN KEY ("BrandId") REFERENCES "Brands" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Cars_Models_ModelId" FOREIGN KEY ("ModelId") REFERENCES "Models" ("Id") ON DELETE CASCADE
);


CREATE TABLE "Ownerships" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Ownerships" PRIMARY KEY AUTOINCREMENT,
    "CarId" INTEGER NOT NULL,
    "OwnerId" INTEGER NOT NULL,
    "CarId1" INTEGER NULL,
    "OwnerId1" INTEGER NULL,
    CONSTRAINT "FK_Ownerships_Cars_CarId" FOREIGN KEY ("CarId") REFERENCES "Cars" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Ownerships_Cars_CarId1" FOREIGN KEY ("CarId1") REFERENCES "Cars" ("Id"),
    CONSTRAINT "FK_Ownerships_Owners_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES "Owners" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Ownerships_Owners_OwnerId1" FOREIGN KEY ("OwnerId1") REFERENCES "Owners" ("Id")
);


CREATE INDEX "IX_Cars_BrandId" ON "Cars" ("BrandId");


CREATE INDEX "IX_Cars_ModelId" ON "Cars" ("ModelId");


CREATE INDEX "IX_Models_BrandId" ON "Models" ("BrandId");


CREATE UNIQUE INDEX "IX_Ownerships_CarId" ON "Ownerships" ("CarId");


CREATE UNIQUE INDEX "IX_Ownerships_CarId1" ON "Ownerships" ("CarId1");


CREATE INDEX "IX_Ownerships_OwnerId" ON "Ownerships" ("OwnerId");


CREATE UNIQUE INDEX "IX_Ownerships_OwnerId1" ON "Ownerships" ("OwnerId1");


