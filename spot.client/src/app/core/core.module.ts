import { NgModule } from '@angular/core';
import { TableModule } from 'primeng/table';
import { MultiSelectModule } from 'primeng/multiselect';
import { InputTextModule } from 'primeng/inputtext';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { CommonModule } from '@angular/common';
import { ButtonModule } from 'primeng/button';

@NgModule({
  declarations: [
  ],
  providers: [
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TableModule,
    MultiSelectModule,
    InputTextModule,
    ProgressSpinnerModule,
    ButtonModule
  ],
  exports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TableModule,
    TableModule,
    MultiSelectModule,
    InputTextModule,
    ProgressSpinnerModule,
    ButtonModule
  ]
})
export class CoreModule { }
