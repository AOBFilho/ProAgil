import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Evento } from '../_models/Evento';

@Injectable({
  providedIn: 'root'
})
export class EventoService {

  urlBase : string = "https://localhost:5001/api/evento";

  constructor(private http: HttpClient) { }

  getAllEvento(): Observable<Evento[]>  {
    return this.http.get<Evento[]>(this.urlBase);
  }

  getByIdEvento(id: number): Observable<Evento>{
    return this.http.get<Evento>(`${this.urlBase}/${id}`);
  }

  getByTemaEvento(tema: string): Observable<Evento[]>{
    return this.http.get<Evento[]>(`${this.urlBase}/getByTema/${tema}`);
  }
}
