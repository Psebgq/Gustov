// product-option.component.ts
import { Component, Inject, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';
import { ToastrService } from 'ngx-toastr';
import { CreateProduct, Product, UpdateProduct } from '../../../../core/interfaces/product.interface';
import { Category } from '../../../../core/interfaces/category.interface';
import { CategoryService } from '../../../../core/services/category.service';
import { ProductService } from '../../../../core/services/product.service';
import { environment } from '../../../../../environments/environment';

interface DialogData {
  operation: string;
  title: string;
  product: Product;
  disableClose: boolean;
}

@Component({
  selector: 'app-product-option',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    MatProgressSpinnerModule,
    MatDividerModule
  ],
  templateUrl: './product-option.component.html',
  styleUrl: './product-option.component.css'
})
export class ProductOptionComponent implements OnInit {
  productForm!: FormGroup;
  categories: Category[] = [];
  isLoading = false;
  isLoadingCategories = true;
  
  // Variables para manejar la imagen
  selectedFile: File | null = null;
  imagePreview: string | null = null;
  currentImageUrl: string | null = null;
  
  private readonly toastrService = inject(ToastrService);
  private readonly fb = inject(FormBuilder);
  
  constructor(
    public dialogRef: MatDialogRef<ProductOptionComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private readonly productService: ProductService, 
    private readonly categoryService: CategoryService
  ) {
    this.initializeForm();
  }

  ngOnInit(): void {
    console.log('Dialog data:', this.data);
    this.loadCategories();
    
    // Diferenciar entre CREATE y UPDATE
    if (this.data.operation === 'edit' && this.data.product) {
      this.populateForm();
      this.setCurrentImage();
    }
    // Para CREATE, el formulario queda vacío y listo para llenar
  }

  private initializeForm(): void {
    this.productForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      categoryId: ['', Validators.required],
      description: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(500)]],
      price: ['', [Validators.required, Validators.min(0.01)]],
      sortOrder: ['', [Validators.required, Validators.min(1)]],
      isActive: [true]
    });
  }

  private populateForm(): void {
    if (this.data.product) {
      this.productForm.patchValue({
        name: this.data.product.name,
        categoryId: this.data.product.categoryId,
        description: this.data.product.description,
        price: this.data.product.price,
        sortOrder: this.data.product.sortOrder,
        isActive: this.data.product.isActive
      });
    }
  }

  private setCurrentImage(): void {
    if (this.data.product?.image) {
      // Construir URL completa si es necesario
      if (this.data.product.image.startsWith('http')) {
        this.currentImageUrl = this.data.product.image;
      } else {
        this.currentImageUrl = `${environment.baseUrl}${this.data.product.image}`;
      }
    }
  }

  private loadCategories(): void {
    this.isLoadingCategories = true;
    
    this.categoryService.findAll().subscribe({
      next: (categories) => {
        this.categories = categories.filter(category => category.isActive);
        this.isLoadingCategories = false;
      },
      error: (error) => {
        console.error('Error loading categories:', error);
        this.toastrService.error('Error al cargar las categorías', 'Error');
        this.isLoadingCategories = false;
      }
    });
  }

  onFileSelected(event: Event): void {
    // Prevenir cualquier comportamiento por defecto
    event.preventDefault();
    event.stopPropagation();
    
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      
      // Validar archivo usando ProductService
      const validation = this.productService.validateImageFile(file);
      if (!validation.isValid) {
        this.toastrService.error(validation.error!, 'Archivo Inválido');
        this.clearFileInput();
        return;
      }

      this.selectedFile = file;
      this.createImagePreview(file);
      
      // Solo mostrar que se seleccionó, NO subir todavía
      this.toastrService.info(`Imagen "${file.name}" lista para cargar`, 'Imagen Seleccionada');
    }
  }

  // Crear preview de la imagen
  private createImagePreview(file: File): void {
    const reader = new FileReader();
    reader.onload = (e: any) => {
      this.imagePreview = e.target.result;
    };
    reader.readAsDataURL(file);
  }

  // Limpiar input de archivo
  clearFileInput(): void {
    this.selectedFile = null;
    this.imagePreview = null;
    // Usar template reference en lugar de getElementById
    const fileInput = document.getElementById('imageInput') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
      // Resetear completamente el input
      fileInput.type = '';
      fileInput.type = 'file';
    }
  }

  // Remover imagen seleccionada
  removeSelectedImage(): void {
    this.clearFileInput();
    this.toastrService.info('Imagen removida', 'Archivo');
  }

  onSubmit(): void {
    if (this.productForm.valid && !this.isLoading) {
      this.isLoading = true;
      
      // Determinar si es CREATE o UPDATE según la operación
      const isCreateOperation = this.data.operation === 'create';
      
      if (isCreateOperation) {
        // CREAR PRODUCTO
        const createData: CreateProduct = {
          categoryId: this.productForm.value.categoryId,
          name: this.productForm.value.name.trim(),
          description: this.productForm.value.description.trim(),
          price: Number(this.productForm.value.price),
          sortOrder: Number(this.productForm.value.sortOrder),
          image: `assets/images/${this.selectedFile?.name || 'default.jpg'}`, // Imagen por defecto si no hay archivo
          isActive: this.productForm.value.isActive
        };

        // Llamar create con o sin imagen
        this.productService.create(createData, this.selectedFile || undefined).subscribe({
          next: (newProduct) => {
            this.toastrService.success(
              `Producto "${newProduct.name}" creado correctamente`, 
              'Producto Creado'
            );
            this.isLoading = false;
            this.dialogRef.close(newProduct);
          },
          error: (error) => this.handleError(error, 'crear')
        });
        
      } else {
        // ACTUALIZAR PRODUCTO
        const updateData: UpdateProduct = {
          categoryId: this.productForm.value.categoryId,
          name: this.productForm.value.name.trim(),
          description: this.productForm.value.description.trim(),
          price: Number(this.productForm.value.price),
          sortOrder: Number(this.productForm.value.sortOrder),
          image: this.data.product.image,
          isActive: this.productForm.value.isActive
        };

        // Solo enviar imagen si hay una nueva seleccionada
        const imageToSend = this.selectedFile || undefined;

        this.productService.update(this.data.product.id, updateData, imageToSend).subscribe({
          next: (updatedProduct) => {
            this.toastrService.success(
              `Producto "${updatedProduct.name}" actualizado correctamente`, 
              'Producto Actualizado'
            );
            this.isLoading = false;
            this.dialogRef.close(updatedProduct);
          },
          error: (error) => this.handleError(error, 'actualizar')
        });
      }
    } else {
      this.markFormGroupTouched();
      this.toastrService.warning('Por favor, completa todos los campos requeridos', 'Formulario Incompleto');
    }
  }

  // Método para manejar errores de forma centralizada
  private handleError(error: any, operation: string): void {
    console.error(`Error ${operation} product:`, error);
    
    let errorMessage = `Error al ${operation} el producto`;
    
    if (error.error) {
      if (typeof error.error === 'string') {
        errorMessage = error.error;
      } else if (error.error.message) {
        errorMessage = error.error.message;
      }
    }
    
    // Errores específicos de HTTP
    switch (error.status) {
      case 400:
        errorMessage = 'Datos del producto inválidos';
        break;
      case 413:
        errorMessage = 'El archivo de imagen es demasiado grande';
        break;
      case 415:
        errorMessage = 'Tipo de archivo no soportado';
        break;
      case 404:
        errorMessage = 'Producto no encontrado';
        break;
      case 409:
        errorMessage = 'Ya existe un producto con ese nombre';
        break;
      case 500:
        errorMessage = 'Error interno del servidor';
        break;
    }
    
    this.toastrService.error(errorMessage, 'Error');
    this.isLoading = false;
  }

  onCancel(): void {
    this.dialogRef.close(null);
  }

  private markFormGroupTouched(): void {
    Object.keys(this.productForm.controls).forEach(key => {
      const control = this.productForm.get(key);
      control?.markAsTouched();
    });
  }

  getErrorMessage(fieldName: string): string {
    const control = this.productForm.get(fieldName);
    
    if (control?.hasError('required')) {
      return `${this.getFieldDisplayName(fieldName)} es requerido`;
    }
    
    if (control?.hasError('minlength')) {
      const minLength = control.errors?.['minlength'].requiredLength;
      return `${this.getFieldDisplayName(fieldName)} debe tener al menos ${minLength} caracteres`;
    }
    
    if (control?.hasError('maxlength')) {
      const maxLength = control.errors?.['maxlength'].requiredLength;
      return `${this.getFieldDisplayName(fieldName)} no puede exceder ${maxLength} caracteres`;
    }
    
    if (control?.hasError('min')) {
      const min = control.errors?.['min'].min;
      return `${this.getFieldDisplayName(fieldName)} debe ser mayor a ${min}`;
    }
    
    return '';
  }

  private getFieldDisplayName(fieldName: string): string {
    const fieldNames: { [key: string]: string } = {
      'name': 'Nombre',
      'categoryId': 'Categoría',
      'description': 'Descripción',
      'price': 'Precio',
      'sortOrder': 'Orden'
    };
    
    return fieldNames[fieldName] || fieldName;
  }

  isFieldInvalid(fieldName: string): boolean {
    const control = this.productForm.get(fieldName);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }

  // Getter para determinar qué imagen mostrar
  get displayImage(): string | null {
    if (this.imagePreview) {
      return this.imagePreview; // Nueva imagen seleccionada
    }
    return this.currentImageUrl; // Imagen actual del producto
  }

  // Getter para verificar si hay una imagen para mostrar
  get hasImage(): boolean {
    return !!(this.imagePreview || this.currentImageUrl);
  }

  // Getter para mostrar el estado de la imagen
  get imageStatus(): string {
    if (this.selectedFile && this.imagePreview) {
      const fileSize = (this.selectedFile.size / 1024 / 1024).toFixed(2);
      const isCreate = this.data.operation === 'create';
      return `${isCreate ? 'Imagen para crear' : 'Nueva imagen'}: ${this.selectedFile.name} (${fileSize} MB)`;
    }
    if (this.currentImageUrl && this.data.operation === 'edit') {
      return 'Imagen actual del producto';
    }
    return 'Sin imagen';
  }

  // Getter para verificar si se puede enviar el formulario
  get canSubmit(): boolean {
    return this.productForm.valid && !this.isLoading;
  }

  // Getter para el texto del botón de envío
  get submitButtonText(): string {
    const isCreate = this.data.operation === 'create';
    const hasNewImage = !!this.selectedFile;
    
    if (this.isLoading) {
      if (hasNewImage) {
        return isCreate ? 'Creando con imagen...' : 'Actualizando con imagen...';
      }
      return isCreate ? 'Creando producto...' : 'Actualizando producto...';
    }
    
    return isCreate ? 'Crear Producto' : 'Actualizar Producto';
  }

  // Método para obtener información del archivo
  getFileInfo(): string {
    if (!this.selectedFile) return '';
    
    const fileSize = (this.selectedFile.size / 1024 / 1024).toFixed(2);
    const fileType = this.selectedFile.type.split('/')[1].toUpperCase();
    
    return `${this.selectedFile.name} • ${fileType} • ${fileSize} MB`;
  }
}