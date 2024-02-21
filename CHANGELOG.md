# Changelog

Todas as mudanças notáveis deste projeto serão documentadas neste arquivo.

Este formato é baseado em [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)
e este projeto adere ao [Versionamento Semântico](https://semver.org/spec/v2.0.0.html).

## [2.2.0] - 2024-02-21

### Corrigido
- Agora, ao criar um Movimento com Registros, mas sem valor em `numberOfMovements`, o valor de `numberOfMovements` será automaticamente atualizado com a quantidade de Registros existentes;
- Erro ao desserializar uma Sessão recebida do servidor que não tivesse valor para `movements` ou `movementIds`.

### Alterado
- A partir deste update, **qualquer usuário que quiser acessar o ReBase precisa ter um token de autenticação cadastrado no sistema**. Para receber um token, o usuário deve entrar em contato com o time de desenvolvimento através dos emails `mrtrotta2010@gmail.com` ou `diegocolombodias@gmail.com` e enviar o endereço de email que deseja cadastrar;
- [BREAKING] A classe `ReBaseClient` deixa de ser singleton e passar a requerer inicialização com o email e o token cadastrados do usuário;
- No cabeçalho das requisições agora são enviados o email cadastrado e o token do usuário nos headers **"ReBase-User-Email"** e **"ReBase-User-Token"**.

## [2.1.1] - 2023-12-20

### Corrigido
- Serialização do atributo `Movement.registers`.

## [2.1.0] - 2023-12-18

### Adicionado
- Suporte ao parâmetro `deep` em `FetchSessions` e `FindSessions`;
- Atributo `movement_ids` em `session`, contendo os IDs dos seus Movimentos. Esse campo é retornado nas requisições `fetch_sessions` e `find_session` quando o parâmetro `deep` está ausente ou tem o valor `False`;
- Indicador da versão atual no topo do arquivo `README.md`.

### Removido
- Código inutilizado.

### Corrigido
- Método `Session.ToJson` incluía o campo `numberOfMovements`, proibido para a criação de Sessões. 

## [2.0.2] - 2023-12-01

### Alterado
- Altera o nome do ReBase REST Server, ou RRS, para apenas **ReBaseRS**.

## [2.0.1] - 2023-11-20

### Corrigido
- Corrige a palavra 'disease', que estava escrita como 'desease';
- Corrige a URL do servidor.

### Alterado
- Renomeia a propriedade `articulationData` da classe `Movement` para `registers`.
