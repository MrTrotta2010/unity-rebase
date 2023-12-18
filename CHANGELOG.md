# Changelog

Todas as mudanças notáveis deste projeto serão documentadas neste arquivo.

Este formato é baseado em [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)
e este projeto adere ao [Versionamento Semântico](https://semver.org/spec/v2.0.0.html).

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
