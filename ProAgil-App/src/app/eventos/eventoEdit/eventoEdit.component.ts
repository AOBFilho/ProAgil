import { Component, OnInit } from '@angular/core';
import { EventoService } from '../../_services/evento.service';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { Evento } from 'src/app/_models/Evento';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-evento-edit',
  templateUrl: './eventoEdit.component.html',
  styleUrls: ['./eventoEdit.component.css']
})

export class EventoEditComponent implements OnInit {
  titulo = 'Edição de Evento';
  registerForm: FormGroup;
  evento: Evento = new Evento();
  imagemURL = 'assets/img/upload.png';
  fileNameToUpdate: string;
  file: File;
  dataAtual = '';

  get lotes(): FormArray {
    return this.registerForm.get('lotes') as FormArray;
  }

  get redesSociais(): FormArray {
    return this.registerForm.get('redesSociais') as FormArray;
  }

  constructor(private eventoService: EventoService,
              private formBuilder: FormBuilder,
              private localeService: BsLocaleService,
              private toastr: ToastrService,
              private router: ActivatedRoute) {
    this.localeService.use('pt-br');
  }

  ngOnInit() {
    this.validation();
    this.carregarEvento();
  }

  carregarEvento() {
    const idEvento = +this.router.snapshot.paramMap.get('id');
    this.eventoService.getByIdEvento(idEvento)
      .subscribe(
        (evento: Evento) => {
          this.evento = Object.assign({}, evento);
          this.fileNameToUpdate = evento.imagemURL.toString();
          this.imagemURL = `https://localhost:5001/Resources/images/${this.evento.imagemURL}?_ts=${this.dataAtual}`;
          this.evento.imagemURL = '';
          this.registerForm.patchValue(this.evento);

          this.evento.lotes.forEach( lote => {
            this.lotes.push(this.criaLote(lote));
          });
          this.evento.redesSociais.forEach( redeSocial => {
            this.redesSociais.push(this.criaRedeSocial(redeSocial));
          });
        }
      );
  }
  onFileChange(evento: any, file: FileList) {
    const reader = new FileReader();
    reader.onload = (event: any) => this.imagemURL = event.target.result;
    this.file = evento.target.files;
    reader.readAsDataURL(file[0]);
  }

  validation() {
    this.registerForm = this.formBuilder.group({
      id: [],
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      qtdePessoas: ['', [Validators.required, Validators.max(120000)]],
      imagemURL: [''],
      lotes: this.formBuilder.array([]) ,
      redesSociais: this.formBuilder.array([])
    });
  }

  criaLote(lote: any): FormGroup {
    return this.formBuilder.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim]
    });
  }

  criaRedeSocial(redeSocial: any): FormGroup {
    return this.formBuilder.group({
      id: [redeSocial.id],
      nome: [redeSocial.nome, Validators.required],
      url: [redeSocial.url, Validators.required]
    });
  }

  adicionarLote() {
    this.lotes.push(this.criaLote({id: 0}));
  }

  adicionarRedeSocial() {
    this.redesSociais.push(this.criaRedeSocial({id: 0}));
  }

  removerLote(id: number) {
    this.lotes.removeAt(id);
  }

  removerRedeSocial(id: number) {
    this.redesSociais.removeAt(id);
  }

  salvarEvento() {
    if (this.registerForm.valid) {
      this.evento = Object.assign({id: this.evento.id}, this.registerForm.value);
      this.evento.imagemURL = this.fileNameToUpdate;
      this.eventoService.putEvento(this.evento).subscribe(
        (evento: Evento) => {
          if (this.registerForm.get('imagemURL').value !== '') {
            this.eventoService.uploadImage(this.file, evento.id.toString()).subscribe(
              () => {
                this.dataAtual = new Date().getMilliseconds().toString();
                this.imagemURL = `https://localhost:5001/Resources/images/${this.evento.imagemURL}?_ts=${this.dataAtual}`;

                this.toastr.success('Evento salvo com sucesso!');
              },
              error => {
                this.toastr.error(`Ocorreu um erro ao tentar salvar evento: ${error.error}`);
              }
            );
          } else {
            this.toastr.success('Evento salvo com sucesso!');
          }
        },
        error => {
          this.toastr.error(`Ocorreu um erro ao tentar salvar evento: ${error.error}`);
        }
      );
    }
  }
}
