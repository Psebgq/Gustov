-- =============================================
-- SCRIPT SIMPLIFICADO PARA MENÚ BOLIVIANO
-- Restaurante Gustov - Sistema POS
-- =============================================

-- Insertar Categorías con IDs específicos
SET IDENTITY_INSERT Categories ON;

INSERT INTO Categories (Id, Name, SortOrder, Description, IsActive, CreatedAt, UpdatedAt)
VALUES 
    (10, 'Platos Principales', 1, 'Platos tradicionales bolivianos', 1, GETDATE(), GETDATE()),
    (11, 'Especialidades', 2, 'Especialidades de la casa', 1, GETDATE(), GETDATE());

SET IDENTITY_INSERT Categories OFF;

-- Insertar Productos con IDs específicos
SET IDENTITY_INSERT Products ON;

INSERT INTO Products (Id, CategoryId, Name, SortOrder, Description, Price, Image, IsActive, CreatedAt, UpdatedAt)
VALUES 
    -- Picante de Pollo
    (100, 10, 'Picante de Pollo', 1, 
     'Delicioso pollo en salsa picante tradicional boliviana, cocido con ají amarillo, cebolla, ajo y especias. Servido con papa y arroz.',
     35.00, 'assets/images/picante-pollo.jpg', 1, GETDATE(), GETDATE()),
    
    -- Charque
    (101, 11, 'Charque', 1,
     'Carne de llama deshidratada y salada, preparada de manera tradicional andina. Se sirve con mote, papa phuti y ají de locoto.',
     42.00, 'assets/images/charque.jpg', 1, GETDATE(), GETDATE()),
    
    -- Pique Macho
    (102, 10, 'Pique Macho', 2,
     'Contundente plato paceño con carne de res y chorizo, papas fritas, huevo frito, tomate, cebolla, locoto y perejil.',
     38.00, 'assets/images/pique-macho.jpg', 1, GETDATE(), GETDATE()),
    
    -- Pailita de Cordero
    (103, 11, 'Pailita de Cordero', 2,
     'Tierno cordero cocido a fuego lento en paila de barro con papas, zanahoria, arvejas y hierbas aromáticas.',
     45.00, 'assets/images/pailita-cordero.jpg', 1, GETDATE(), GETDATE());

SET IDENTITY_INSERT Products OFF;