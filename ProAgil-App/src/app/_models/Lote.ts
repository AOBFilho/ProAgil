export interface Lote {
    id: number;
    nome: string; 
    preco: number; 
    dataInicial?: Date; 
    dataFinal?: Date;
    quantidade: number;
    eventoId: number;
}
