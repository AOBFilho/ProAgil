import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Evento } from '../_models/Evento';

@Injectable({
  providedIn: 'root'
})
export class EventoService {

  urlBase = 'https://localhost:5001/api/evento';

  constructor(private http: HttpClient) {}

  getAllEvento(): Observable<Evento[]>  {
    return this.http.get<Evento[]>(this.urlBase);
  }

  getByIdEvento(id: number): Observable<Evento> {
    return this.http.get<Evento>(`${this.urlBase}/${id}`);
  }

  getByTemaEvento(tema: string): Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.urlBase}/getByTema/${tema}`);
  }

  postEvento(evento: Evento) {
    return this.http.post(this.urlBase, evento);
  }

  putEvento(evento: Evento) {
    return this.http.put(`${this.urlBase}/${evento.id}`, evento);
  }

  deleteEvento(id: number) {
    return this.http.delete(`${this.urlBase}/${id}`);
  }

  uploadImage(file: File, name: string) {
    const fileToUpload = file[0] as File;
    const formData = new FormData();
    formData.append('file', fileToUpload, `${name}.jpg`);
    return this.http.post(`${this.urlBase}/uploadImage`, formData);
  }
}
