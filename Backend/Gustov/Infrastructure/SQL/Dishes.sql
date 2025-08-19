-- Script SQL para insertar platillos bolivianos en la base de datos del Restaurante Gustov

INSERT INTO Categories (Name, SortOrder, Description, IsActive, CreatedAt, UpdatedAt)
VALUES 
    ('Platos Principales', 1, 'Platos tradicionales bolivianos', 1, GETDATE(), GETDATE()),
    ('Especialidades', 2, 'Especialidades de la casa', 1, GETDATE(), GETDATE());

-- Variables para obtener los IDs de las categorías
DECLARE @PlatosPrincipalesId INT = (SELECT Id FROM Categories WHERE Name = 'Platos Principales');
DECLARE @EspecialidadesId INT = (SELECT Id FROM Categories WHERE Name = 'Especialidades');

-- Insertar los platillos bolivianos
INSERT INTO Products (CategoryId, Name, SortOrder, Description, Price, Image, IsActive, CreatedAt, UpdatedAt)
VALUES 
    -- Picante de Pollo
    (@PlatosPrincipalesId, 
     'Picante de Pollo', 
     1, 
     'Delicioso pollo en salsa picante tradicional boliviana, cocido con ají amarillo, cebolla, ajo y especias. Servido con papa y arroz. Un clásico de la gastronomía cochabambina que despierta todos los sentidos con su sabor intenso y aromático.',
     35.00,
     'assets/images/picante-pollo.jpg',
     1,
     GETDATE(),
     GETDATE()),

    -- Charque
    (@EspecialidadesId,
     'Charque',
     1,
     'Carne de llama deshidratada y salada, preparada de manera tradicional andina. Se sirve con mote (maíz pelado), papa phuti y ají de locoto. Una experiencia gastronómica única que representa la herencia culinaria de los pueblos originarios del altiplano.',
     42.00,
     'assets/images/charque.jpg',
     1,
     GETDATE(),
     GETDATE()),

    -- Pique
    (@PlatosPrincipalesId,
     'Pique Macho',
     2,
     'Contundente plato paceño con carne de res y chorizo cortados en trozos pequeños, papas fritas, huevo frito, tomate, cebolla, locoto y perejil. Perfecto para compartir. Una explosión de sabores que satisface hasta el apetito más exigente.',
     38.00,
     'assets/images/pique-macho.jpg',
     1,
     GETDATE(),
     GETDATE()),

    -- Pailita (Paila)
    (@EspecialidadesId,
     'Pailita de Cordero',
     2,
     'Tierno cordero cocido a fuego lento en paila de barro con papas, zanahoria, arvejas y hierbas aromáticas. Preparación tradicional que conserva todos los jugos naturales de la carne. Acompañado con llajua y pan de batalla para una experiencia completa.',
     45.00,
     'assets/images/pailita-cordero.jpg',
     1,
     GETDATE(),
     GETDATE()),

    -- Bonus: Variante de Pique
    (@PlatosPrincipalesId,
     'Pique a lo Pobre',
     3,
     'Versión tradicional del pique con carne de res, papas fritas, huevo frito, arroz, plátano frito y ensalada fresca. Ideal para quienes buscan un plato abundante y nutritivo con el auténtico sabor boliviano.',
     33.00,
     'assets/images/pique-pobre.jpg',
     1,
     GETDATE(),
     GETDATE());

-- Verificar que se insertaron correctamente
SELECT 
    c.Name AS Categoria,
    p.Name AS Platillo,
    p.Description AS Descripcion,
    p.Price AS Precio,
    p.Image AS Imagen
FROM Products p
INNER JOIN Categories c ON p.CategoryId = c.Id
WHERE p.Name IN ('Picante de Pollo', 'Charque', 'Pique Macho', 'Pailita de Cordero', 'Pique a lo Pobre')
ORDER BY c.Name, p.SortOrder;