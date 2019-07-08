import { Component, OnInit } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { FormGroup, Validators, FormBuilder, AbstractControl } from '@angular/forms';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { ToastrService } from 'ngx-toastr';
import { FormatDateTimePipe } from '../_helps/FormatDateTime.pipe';
import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';

registerLocaleData(localePt);
defineLocale('pt-br', ptBrLocale); 

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})

export class EventosComponent implements OnInit {
  dataEvento: any;
  titulo = 'Eventos';
  eventos: Evento[];
  evento: Evento;
  modoSalva: string = '';
  imagemAltura = 50;
  imagemMargem = 2;
  mostrarImagem = false;
  registerForm: FormGroup;
  _eventosFiltrados: Evento[];
  _filtroLista: string;
  headerTextDelete: string = '';
  file: File;
  
  constructor(private eventoService: EventoService,
              private formBuilder: FormBuilder,
              private localeService: BsLocaleService,
              private toastr: ToastrService) {
    this.localeService.use('pt-br');
  }
    
    
  get filtroLista():string {
    return this._filtroLista;
  }

  get eventosFiltrados(): Evento[]{
    return this._eventosFiltrados ? this._eventosFiltrados : this.eventos ;
  }
  
  set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEvento(this.filtroLista) : this.eventos; 
  }
  
  set eventosFiltrados(value: Evento[]){
    this._eventosFiltrados = value;
  }
  
  ngOnInit() {
    this.validation();
    this.getEventos();
  }
  
  openModal(template: any){
    this.registerForm.reset();
    template.show();
  }
  
  filtrarEvento(filtroLista: string): Evento[] {
    filtroLista = filtroLista.toLocaleLowerCase();
    return this.eventos.filter(evento => evento.tema.toLocaleLowerCase().indexOf(filtroLista) !== -1);
  }
  
  getEventos(){
    this.eventoService.getAllEvento().subscribe(
      (_evento: Evento[]) => {
        this.eventos = _evento;
      },
      error => {
        console.log(error);
        this.toastr.error(`Ocorreu um erro ao carregar eventos: ${error.error}`);
      }
    )
  }
    
  alternarImagem(){
    this.mostrarImagem = !this.mostrarImagem;
  }

  salvarAlteracao(template: any){
    if (this.registerForm.valid){
      this.pesistirEvento().subscribe(
        (evento: Evento) => {
          
          this.eventoService.uploadImage(this.file,evento.id.toString()).subscribe(
            () => {
              template.hide();
              this.getEventos();
              this.toastr.success('Evento salvo com sucesso!');
            },
            error => {
              this.toastr.error(`Ocorreu um erro ao tentar salvar evento: ${error.error}`);
            }
          )
        },
        error => {
          this.toastr.error(`Ocorreu um erro ao tentar salvar evento: ${error.error}`);
        }
      )

    }
  }
  
  pesistirEvento() {
    if (this.modoSalva === 'put'){
      this.evento = Object.assign({id: this.evento.id},this.registerForm.value);
      return this.eventoService.putEvento(this.evento);
    } else if (this.modoSalva === 'post'){
      this.evento = Object.assign({},this.registerForm.value);
      return this.eventoService.postEvento(this.evento);
    }
  }
  
  novoEvento(template: any){
    this.modoSalva = 'post';
    this.openModal(template);
  }
  
  editarEvento(evento:Evento, template: any){
    var datePipe = new FormatDateTimePipe('pt-br')
    this.modoSalva = 'put';
    this.openModal(template);
    this.evento = Object.assign({},evento);
    this.evento.imagemURL = '';
    this.registerForm.patchValue(this.evento);
  }
  
  excluirEvento(evento: Evento,confirm : any){
    this.headerTextDelete = evento.tema;
    confirm.show();
    this.evento = evento;
  }
  
  confirmaExclusao(confirm : any){
    this.eventoService.deleteEvento(this.evento.id).subscribe(
      () => {
        confirm.hide();
        this.getEventos();
        this.toastr.success('Evento excluido com sucesso!');
      },
      (error) => {
        this.toastr.error(`Ocorreu um erro ao tentar excluir o evento: ${error.error}`);
      }
    );
  }
        
  validation(){
    this.registerForm = this.formBuilder.group({
      tema: ['',[Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['',Validators.required],
      dataEvento: ['',Validators.required],
      telefone: ['',Validators.required],
      email: ['',[Validators.required,Validators.email]],
      qtdePessoas: ['',[Validators.required,Validators.max(120000)]],
      imagemURL: ['',Validators.required]
    });
  }

  onFileChange(event){
    if (event.target.files && event.target.files.length){
      this.file = event.target.files;
    }
  }
  
  getControl(nomeControl: string) : AbstractControl{
    return this.registerForm.get(nomeControl);
  }
  
  isFormControlInvalid(nomeControl: string): boolean{
    var control = this.getControl(nomeControl); 
    return control.invalid && (control.dirty || control.touched); 
  }
  
  isFormControlRequired(nomeControl: string): boolean{
    return this.getControl(nomeControl).hasError('required');
  }
  
  isFormControlMinLength(nomeControl: string): boolean{
    return this.getControl(nomeControl).hasError('minlength');
  }
  
  isFormControlMaxLength(nomeControl: string): boolean{
    return this.getControl(nomeControl).hasError('maxlength');
  }
  
  isFormControlMax(nomeControl: string): boolean{
    return this.getControl(nomeControl).hasError('max');
  }
  
  isFormControlEmailInvalid(nomeControl: string): boolean{
    return this.getControl(nomeControl).hasError('email');
  }
}
        