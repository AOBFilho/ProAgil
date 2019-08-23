import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Evento } from '../_models/Evento';

@Injectable({
  providedIn: 'root'
})
export class EventoService {

  urlBase : string = "https://localhost:5001/api/evento";
  tokenHeader : HttpHeaders;

  constructor(private http: HttpClient) { 
    this.tokenHeader = new HttpHeaders({'Authorization': `Bearer ${localStorage.getItem('token')}` });
  }

  getAllEvento(): Observable<Evento[]>  {
    return this.http.get<Evento[]>(this.urlBase, {headers: this.tokenHeader});
  }

  getByIdEvento(id: number): Observable<Evento>{
    return this.http.get<Evento>(`${this.urlBase}/${id}`);
  }

  getByTemaEvento(tema: string): Observable<Evento[]>{
    return this.http.get<Evento[]>(`${this.urlBase}/getByTema/${tema}`);
  }

  postEvento(evento: Evento){
    return this.http.post(this.urlBase,evento,{headers: this.tokenHeader});
  }

  putEvento(evento: Evento){
    return this.http.put(`${this.urlBase}/${evento.id}`,evento);
  }

  deleteEvento(id: number){
    return this.http.delete(`${this.urlBase}/${id}`);
  }

  uploadImage(file: File, name: string){
    var fileToUpload = <File>file[0];
    var formData = new FormData();
    formData.append('file',fileToUpload,`${name}.jpg`)
    return this.http.post(`${this.urlBase}/uploadImage`,formData, {headers: this.tokenHeader});
  }
}
