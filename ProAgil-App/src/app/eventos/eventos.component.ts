import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  eventos : Evento[];
  imagemAltura = 50;
  imagemMargem = 2;
  mostrarImagem = false;
  modalRef: BsModalRef;
  _eventosFiltrados: Evento[];
  _filtroLista : string;
 
  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService  ) { }

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
    this.getEventos();
  }

  openModal(template: TemplateRef<any>){
    this.modalRef = this.modalService.show(template);
  }

  filtrarEvento(filtroLista: string): Evento[] {
    filtroLista = filtroLista.toLocaleLowerCase();
    return this.eventos.filter(
      evento => evento.tema.toLocaleLowerCase().indexOf(filtroLista) !== -1
    )
  }

  getEventos(){
    this.eventoService.getAllEvento().subscribe(
      (_evento: Evento[]) => 
      {
        this.eventos = _evento;
        console.log(_evento)
      },
      error => {console.log(error)}
    )
  }

  alternarImagem(){
    this.mostrarImagem = !this.mostrarImagem;
  }
}
