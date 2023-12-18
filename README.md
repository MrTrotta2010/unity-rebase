# Unity ReBase [2.1.0]
Este projeto é uma API escrita em C# para uso no Unity, para comunicação com o ReBase, um banco de dados de sessões de reabilitação física.

## Índice
- [Unity ReBase \[2.1.0\]](#unity-rebase-210)
  - [Índice](#índice)
  - [Visão Geral](#visão-geral)
    - [Sobre o ReBase](#sobre-o-rebase)
  - [Instalação](#instalação)
  - [Requisitos](#requisitos)
  - [Quick Start](#quick-start)
    - [Criando um Movimento](#criando-um-movimento)
    - [Criando uma Sessão](#criando-uma-sessão)
    - [Buscando Movimentos e Sessões](#buscando-movimentos-e-sessões)
    - [Atualizando e deletando](#atualizando-e-deletando)
  - [Tópicos Avançados](#tópicos-avançados)
    - [Filtragem](#filtragem)
    - [Paginação](#paginação)
  - [Samples](#samples)
  - [Documentação Completa](#documentação-completa)
    - [ReBaseClient](#rebaseclient)
    - [Modelos](#modelos)
      - [APIResponse](#apiresponse)
      - [Movement](#movement)
      - [Register](#register)
      - [Rotation](#rotation)
      - [Session](#session)
    - [Miscelânea](#miscelânea)
      - [MetaData](#metadata)
      - [SerializableMovement](#serializablemovement)
        - [AppData](#appdata)
      - [SerializableSession](#serializablesession)
        - [PatientData](#patientdata)
        - [MedicalData](#medicaldata)
    - [Exceções](#exceções)


## Visão Geral
O pacote Unity ReBase contém classes-modelo para Sessões e Movimentos, além de versões serializáveis destas classes. A classe `APIResponse` modela de forma generalizada as respostas enviadas pelo `ReBaseRS`. Todos os modelos se encontram na pasta `Runtime/Models`. Estão presentes também algumas exceções personalizadas, localizadas na pasta `Runtime/Exceptions`. A classe `ReBaseClient` é responsável pelo envio de requisições ao ReBaseRS e se encontra na raiz da pasta `Runtime`. Por fim, outras classes diversas encontram-se na pasta `Runtime/Misc`.

### Sobre o ReBase
O ReBase, do inglês *Rehabilitation Database*, é um baco de dados dedicado ao armazenamento de movimentos corporais, com foco em reabilitação neurofuncional e neuromotora. Apesar do enfoque, o ReBase é capaz de armazenar qualquer tipo de movimento corporal gravado por qualquer técnica de captura de movimentos, desde que siga o padrão definido. Para isto serve a API Unity ReBase!

Os **Movimentos** do ReBase representam os movimentos corporais capturados e são compostos por metadados, uma lista de *Articulações* e uma lista de **Registros**, que representam as rotações em X, y e z da Articulação a cada instante do Movimento. Os Movimentos podem pertencer a **Sessões**. Cada Sessão também contem metadados e pode conter múltiplos movimentos.

## Instalação
1. Baixe o arquivo .zip;
2. Extraia a pasta compactada;
3. No editor do Unity, [instale o pacote a partir do disco](https://docs.unity3d.com/Manual/upm-ui-local.html).

## Requisitos
* Este pacote utiliza a biblioteca [Newtonsoft Json 3.2](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@3.2/manual/index.html) e portanto suporta o editor **2018.4 ou superior**;
* Este pacote também depende da classe `System.Net.Http` e do paradigma `Async/Await`, por isso requer pelo menos **C# 5** e [uma destas versões do pacote .NET](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-7.0#applies-to).

## Quick Start
Para utilizar a API, basta incluir a biblioteca adicionando `using ReBase` no começo do seu arquivo .cs

### Criando um Movimento
```C#
// Crie um objeto Movement
Movement movement = new Movement(
    label: "MyMovement",
    fps: Application.targetFrameRate,
    professionalId: "Prof",
    articulations: new string[] { "1", "2" }
);

// Adicione os Registros ao Movimento
// Cada registro representa um "frame" do movimento
// Os registros contêm as rotações de cada articulação em um dado momento 
movement.AddRegister(new Register(
    new Dictionary<string, Rotation>()
    {
        { "1", new Rotation(1f, 1f, 1f) }, // Rotações da articulação "1"
        { "2", new Rotation(2f, 2f, 2f) } // Rotações da articulação "2"
    }
));

// Utilize a classe singleton ReBaseClient para inserir o movimento
// Todas as requisições são assíncronas.
APIResponse response = await ReBaseClient.Instance.InsertMovement(movement);
Debug.Log($"Inserted: {response}");
```

### Criando uma Sessão
```C#
// Crie um objeto Sessão.
// A Sessão pode ser criada vazia ou com Movimentos
session = new Session(
    title: "Teste de Sessão",
    professionalId: "Prof",
    patientId: "Pat",
    movements: new Movement[2]
    {
        // Da mesma forma, os Movimentos podem ser criados já com Registros
        new Movement(
            label: "MyMovement",
            fps: Application.targetFrameRate,
            articulations: new string[] { "1", "2" },
            registers: new Register[1] {
                new Register(
                    new Dictionary<string, Rotation>()
                    {
                        { "1", new Rotation(1f, 1f, 1f) },
                        { "2", new Rotation(2f, 2f, 2f) }
                    }
                )
            }
        ),
        new Movement(
            label: "MyMovement",
            fps: Application.targetFrameRate,
            articulations: new string[] { "1", "2" },
            registers: new Register[1] {
                new Register(
                    new Dictionary<string, Rotation>()
                    {
                        { "1", new Rotation(1f, 1f, 1f) },
                        { "2", new Rotation(2f, 2f, 2f) }
                    }
                )
            }
        )
    }
);

APIResponse response = await ReBaseClient.Instance.InsertSession(session);
```

### Buscando Movimentos e Sessões
```C#
// É possível encontrar um único Movimento ou listar vários
APIResponse response = await ReBaseClient.Instance.FindMovement(id);
Debug.Log($"Movimento: {response.movement}");

// A listagem permite filtros e suporta paginação
// Os filtros possíveis são: professionalId (id do profissional de saúde), patientId (id do paciente),
// movementLabel (identificação do movimento), articulations (articulações incluídas no movimento)
response = await ReBaseClient.Instance.FetchMovements(professionalId: "professional", patientId: "patient", page: 1, per: 10);
Debug.Log($"Movimentos: {response.movements}");

// Da mesma forma, as Sessões podem ser buscas individualmente ou listadas
response = await ReBaseClient.Instance.FindSession(id);
Debug.Log($"Sessão: {response.session}");

// Os filtros suportados pela listagem de Sessão são: professionalId e patientId.
// Também são aceitos os filtros movementLabel e articulations, mas estes filtram os movimentos das Sessões
response = await ReBaseClient.Instance.FetchSessions(professionalId: "professional", patientId: "patient", page: 1, per: 10);
Debug.Log($"Sessões: {response.sessions}");
```

### Atualizando e deletando
```C#
// São disponibilizados métodos para deletar e excluir Movimentos e Sessões
APIResponse response = await ReBaseClient.Instance.UpdateMovement(id, updatedMovement);
response = await ReBaseClient.Instance.DeleteMovement(id);

response = await ReBaseClient.Instance.UpdateSession(id, updatedSession);
response = await ReBaseClient.Instance.DeleteSession(id);
```

## Tópicos Avançados
A seguir, serão abordados dois assuntos relevantes para as requisições de listagem: **filtragem** e **paginação**.

### Filtragem
As requisições de listagem do ReBase, de Movimentos ou de Sessões, suportam alguns filtros, listados e explicados a seguir.

Listagem de Movimentos:
* **professionalId:** recebe uma `string` que representa o ID do profissional de saúde responsável pelo Movimento;
* **patientId:** recebe uma `string` que representa o ID do paciente que executou o Movimento;
* **movementLabel:** recebe uma `string` que representa a propriedade `label` do Movimento;
* **articulations:** recebe uma lista de `strings` que representam as articulações trabalhadas no Movimento.

Listagem de Sessões:
* **professionalId:** recebe uma `string` que representa o ID do profissional de saúde responsável pela Sessão;
* **patientId:** recebe uma `string` que representa o ID do paciente que avaliado durante a Sessão.

A listagem de Sessões pode recebe ainda dois filtros adicionais. Estes, no entanto, não filtrarão a lista de Sessões em si, mas sim as listas de Movimentos das Sessões. São eles:
* **movementLabel:** recebe uma `string` que representa a propriedade `label` do Movimento;
* **articulations:** recebe uma lista de `strings` que representam as articulações trabalhadas no Movimento.

Estes filtros podem ser úteis em alguns casos específicos. Um exemplo de uso: listar as Sessões de um paciente específico, mas incluir apenas os Movimentos que trabalharam um grupo específico de articulações.

Vale notar, por fim, que todos os filtros são **aditivos**, ou seja, ao utilizar `n` múltiplos filtros, serão retornados os documentos que satisfaçam **todos** os filtros, ou seja, os documentos que satisfaçam o filtro 1 **E** o filtro 2 **E** ... **E** o filtro n. 

### Paginação
As requisições de listagem, tanto de Movimentos quanto de Sessões, também suportam paginação. Utilizando paginação, elimina-se a necessidade de carregar todos os items de uma listagem de uma só vez, o que pode significar um ganho de velocidade e desempenho para uma aplicação. O ReBaseRS suporta dois tipos de paginação: baseada em **per e page** e baseada em **IDs**.

O primeiro tipo utiliza dois parâmetros: `per`, que representa a quantidade de itens presentes em cada página, e `page`, que representa qual página deve ser carregada. Desta forma, supondo que quiséssemos carregar 10 elementos por página, para carregar a primeira página usaríamos `{ per: 10, page: 1 }` e receberíamos os primeiros 10 itens da lista. Para carregar a segunda página usaríamos `{ per: 10, page: 2 }` e receberíamos os 10 itens seguintes e assim por diante. Este método é simples de se utilizar e de se entender e permite o carregamento de qualquer página, porém possui uma desvantagem: para recuperar uma página `n`, é necessário recuperar todas as páginas anteriores, o que faz com que as requisições fiquem mais lentas conforme o número `n` da página cresce. Para mitigar este problema, é possível utilizar a paginação baseada em IDs.

A paginação baseada em IDs também depende de dois parâmetros: `per`, a quantidade de itens por página, e `previous_id`, o ID do último item da página anterior. Pela forma como o banco de dados do ReBase é modelado, as listas de Movimentos e de Sessões são ordenadas pelos IDs dos documentos, que são sequenciais. Ou seja, se quisermos recuperar os 10 itens seguintes a um item `i`, basta recuperar os primeiros 10 itens cujo ID seja maior do que o ID de `i`. Explorando essa característica, é possível implementar um método de paginação que permite que todas as páginas sejam recuperadas em tempos equivalentes. Desta forma, para recuperar a primeira página usaríamos `{ per: 10, previous_id: null }`, já que não existe uma página anterior. Supondo que o ID do último item da primeira página seja "ABC", para recuperar a segunda página usaríamos `{ per: 10, previous_id: "ABC" }`. Apesar de tudo, este método de paginação também tem uma desvantagem: para recuperar uma página específica, é necessário ter recuperado todas as páginas anteriores a ela, já que é preciso saber os IDs dos documentos que estão contidos nelas.

Assim, cabe ao desenvolvedor analisar sua aplicação e decidir qual método de paginação deverá ser utilizado (ou se a paginação é mesmo necessária). Aplicações que devem permitir a qualquer momento o acesso a qualquer página, deverão usar a paginação baseada em per e page. Por outro lado, para uma aplicação que precisa exibir uma lista com scroll infinito, por exemplo, na qual os itens são carregados sequencialmente, se beneficiará mais do método baseado em IDs. Para mais informações, [este artigo](https://medium.com/swlh/mongodb-pagination-fast-consistent-ece2a97070f3) pode ser útil. 

## Samples
Este pacote inclui uma cena com alguns exemplos adicionais de utilização da API. A cena e os códigos relacionados se encontram na paste `Samples~/UsageExample`

## Documentação Completa
A seguir, estão incluídas tabelas e descrições detalhando todas as classes da API Unity ReBase.

### ReBaseClient
Esta classe é responsável por toda a comunicação com o ReBaseRS. A classe `ReBaseClient` é uma classe estática que implementa o padrão singleton. As requisições seguem o paradigma [async/await de programação assíncrona](https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/) no lugar de callbacks. Os métodos são do tipo `Task<APIResponse>`. Objetos da classe [Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task?view=net-7.0), de forma muito rude, representam um método ainda em execução, portanto é preciso esperar o fim da execução antes de poder acessar seu resultado. É possível aguardar a execução de uma `Task` usando o comando `await`. Ao final da execução das `Tasks`, elas retornarão um objeto da classe [APIResponse](#apiresponse). Exemplos de uso podem ser encontrados na seção [Quick Start:](#quick-start). 

**Atributos:**
| Atributo     | Tipo                    |
| :----------- | ----------------------: |
| **Instance** | **static ReBaseClient** |
| Retorna a instância singleton do `ReBaseRestClient` |

**Métodos:**
| Método             | Tipo                        | Parâmetros                         |
| :----------------- | :-------------------------- | ---------------------------------: |
| **FetchMovements** | **async Task<APIResponse>** |**string professionalId = "", string patientId = "", string movementLabel = "", string[] articulations = null, bool legacy = false, int page = 0, int per = 0, string previousId = ""** |
| Recupera uma lista de Movimentos armazenados no ReBaseRS. Suporta diversos filtros e paginação |
| **FindMovement**   | **async Task<APIResponse>** | **string id, bool legacy = false** |
| Recupera um Movimento específico a partir do ID. O parâmetro `legacy`, se `true`, retorna o Movimento no formato antigo do ReBase |
| **InsertMovement** | **async Task<APIResponse>** | **[Movement](#movement) movement** |
| Insere um Movimento no ReBase                                                         |
| **UpdateMovement** | **async Task<APIResponse>** | **string id, [Movement](#movement) movement**   |
| **UpdateMovement** | **async Task<APIResponse>** | **[Movement](#movement) movement** |
| Atualiza um Movimento já existente no ReBase                                          |
| **DeleteMovement** | **async Task<APIResponse>** | **string id**                      |
| Exclui um Movimento do ReBase                                                         |
| **FetchSessions**  | **async Task<APIResponse>** | **string professionalId = "", string patientId = "", string movementLabel = "", string[] articulations = null, bool legacy = false, int page = 0, int per = 0, string previousId = ""** |
| Recupera uma lista de Sessões armazenadas no ReBaseRS. Suporta diversos filtros e paginação |
| **FindSession**    | **async Task<APIResponse>** | **string id, bool legacy = false** |
| Recupera uma Sessão específica a partir do ID. O parâmetro `legacy`, se `true`, retorna a Sessão no formato antigo do ReBase |
| **InsertSession**  | **async Task<APIResponse>** | **[Session](#session) session**    |
| Insere uma Sessão no ReBase                                                           |
| **UpdateSession**  | **async Task<APIResponse>** | **[Session](#session) session**    |
| Atualiza uma Sessão já existente no ReBase                                            |
| **DeleteSession**  | **async Task<APIResponse>** | **string id, bool deep = false**   |
| Exclui uma Sessão do ReBase                                                           |

### Modelos

#### APIResponse
A classe APIResponse modela uma resposta generalizada do servidor ReBaseRS.

**Atributos:**
| Atributo         | Tipo                       |
| :--------------- | -------------------------: |
| **responseType** | **enum**                   |
| Tipo da requisição enviada. Valores possíveis: { 0: FetchMovements, 1: FindMovement, 3: InsertMovement, 4: UpdateMovement, 5: DeleteMovement, 6: FetchSessions, 7: FindSession, 8: InsertSession, 9: UpdateSession, 10: DeleteSession, 11: APIError } |
| **status**       | **int**                    |
| Status da resposta. 0 representa sucesso e qualquer outro valor representa um erro |
| **success**      | **bool**                   |
| Diz se a requisição foi bem sucedida ou não. Em outras palavras, diz se status == 0 |
| **code**         | **long**                   |
| Código HTTP da resposta (E.g. 200, 201, 404, etc. ) |
| **message**      | **string**                 |
| Mensagem que descreve a resposta      |
| **HTMLError**    | **string**                 |
| Caso a resposta seja um erro que não aconteceu no servidor, poderá retornar um HTML. Neste caso, o HTML estará nesta propriedade |
| **error**        | **string[]**               |
| Caso a resposta seja de erro, apresenta o erro em questão |
| **warning**      | **string[]**               |
| Apresenta possíveis inconsistências encontradas na requisição pelo ReBaseRS, mas que não são críticas a ponto de provocar um erro. Por exemplo, ao criar um Movimento, caso algum campo enviado esteja vazio o ReBaseRS completará a requisição, mas retornará um warning correspondente avisando que o campo em questão foi excluído |
| **movements**    | **[SerializableMovement](#serializablemovement)[]** |
| Possível lista de Movimentos retornada pelo ReBaseRS |
| **movement**     | **[SerializableMovement](#serializablemovement)**   |
| Possível Movimento retornado pelo ReBaseRS |
| **sessions**     | **[SerializableSession](#serializablesession)[]**  |
| Possível lista de Sessão retornada pelo ReBaseRS |
| **session**      | **[SerializableSession](#serializablesession)**    |
| Possível Sessão retornada pelo ReBaseRS**    |
| **deletedId**    | **string**                 |
| Em requisições do tipo DELETE, esta propriedade contém o ID do documento excluído |
| **deletedCount** | **int**                    |
| Em requisições do tipo DELETE, contém o número de documentos excluídos. Por exemplo, ao deletar uma Sessão, é possível também excluir todos os seus Movimentos |
| **meta**         | **[MetaData](#metadata)**  |
| Metadados sobre o recurso em questão e sobre paginação. Presente em respostas de requisições de listagem |

#### Movement
Modela um Movimento do ReBase.

**Atributos:**
| Atributo              | Tipo               |
| :-------------------- | -----------------: |
| **id**                | **string**         |
| ID do Movimento                            |
| **label**             | **string**         |
| Label, ou etiqueta, de identificação do Movimento. Normalmente representa uma categoria ou grupo de Movimentos |
| **description**       | **string**         |
| Descrição do Movimento                     |
| **device**            | **string**         |
| Dispositivo responsável pela captura do Movimento (E.g. Kinect, MediaPipe, etc.) |
| **fps**               | **float**          |
| Quantidade de quadros por segundo do Movimento |
| **duration**          | **float**          |
| Duração do Movimento                       |
| **numberOfRegisters** | **int**            |
| Quantidade de Registros presentes no Movimento (os Registros serão descritos a seguir) |
| **articulations**     | **string[]**       |
| Articulações utilizadas pelo Movimento. As Articulações são identificadas por strings arbitrárias definidas pelo usuário da API. Cabe a cada desenvolvedor definir e seguir seu padrão. Desta maneira, o ReBase pode armazenar movimentos com um conjunto variável de Articulações |
| **insertionDate**     | **string**         |
| Data de inserção do Movimento              |
| **updateDate**        | **string**         |
| Data da última atualização do Movimento    |
| **sessionId**         | **string**         |
| ID da Sessão à qual o Movimento pertence   |
| **professionalId**    | **string**         |
| Identificação do profissional da saúde responsável pela captura do Movimento |
| **appCode**           | **string**            |
| Código da aplicação que gerou o Movimento. Um código arbitrário definido pelos desenvolvedores da aplicação |
| **appData**           | **string**         |
| Dados arbitrários utilizados pela aplicação que gerou o Movimento. Pode ser utilizado pelos usuários da API para armazenar dados no formato que quiserem, como um json ou uma string |
| **patientId**         | **string**         |
| Identificação anônima do paciente que realizou o Movimento |
| **registers**  | **List<[Register](#register)>** |
| Lista de registros que representam o Movimento em si |

**Métodos:**
| Método                       | Tipo            | Parâmetros                         |
| :--------------------------- | :-------------- | ---------------------------------: |
| **AddRegister**              | **void**        | **[Register](#register) register**              |
| Adiciona um novo Registro ao Movimento                                              |
| **ToJson**                   | **string**      | **bool update (default: false)**   |
| Converte o Movimento para json. O parâmetro update modifica o json gerado: para requisições de criação use update como `false` e para requisições de atualização use update como `true` |
| **ToCreateSessionJson**      | **string**      |                                    |
| Converte o Movimento para o json a ser utilizado ao criar uma Sessão com Movimentos |
| **CompareArticulationLists** | **static bool** | **string[] listA, string[] listB** |
| Compara duas listas de Articulações e retorna `true` se forem iguais ou `false` se forem diferentes |

#### Register
Modela um Registro do ReBase, essencialmente um quadro, ou um frame, de um Movimento. Um Registro contém as rotações nos eixos x, y e z de cada Articulação do Movimento em um determinado instante.

**Atributos:**
| Atributo              | Tipo               |
| :-------------------- | -----------------: |
| **articulationCount** | **int**            |
| A quantidade de Articulações presentes no Registro |
| **articulations**     | **string[]**       |
| A lista das Articulações presentes no Registro |
| **isEmpty**           | **bool**           |
| Indica se o registro está vazio, ou seja, não contem nenhuma Articulação |

**Métodos:**
| Método                       | Tipo         | Parâmetros                                  |
| :--------------------------- | :----------- | ------------------------------------------: |
| **SetArticulations**         | **void**     | **string[] articulationList**               |
| Inicializa as Articulações do registro a partir de uma lista de Articulações              |
| **SetArticulationRotations** | **void**     | **string articulation, [Rotation](#rotation) rotations** |
| Atribui Rotações a uma Articulação específica                                             |
| **GetArticulationRotations** | **[Rotation](#rotation)** | **string articulation**                     |
| Retorna as Rotações de uma Articulação específica                                         |

#### Rotation
Representa um conjunto de rotações de uma Articulação.

**Atributos:**
| Atributo | Tipo       |
| :------- | ---------: |
| **x**    | **double** |
| A rotação no eixo X   |
| **y**    | **double** |
| A rotação no eixo Y   |
| **z**    | **double** |
| A rotação no eixo Z   |

**Métodos:**
| Método        | Tipo        | Parâmetros |
| :------------ | :---------- | ---------: |
| **ToVector3** | **Vector3** |            |
| Converte a Rotação para um objeto da classe ``UnityEngine.Vector3`` |

#### Session
Modela uma Sessão do ReBase.

**Atributos:**
| Atributo                    | Tipo               |
| :-------------------------- | -----------------: |
| **id**                      | **string**         |
| ID da Sessão                                     |
| **title**                   | **string**         |
| Título da Sessão                                 |
| **description**             | **string**         |
| Descrição da Sessão                              |
| **professionalId**          | **string**         |
| Identificação do profissional de saúde responsável pela captura da Sessão |
| **patientSessionNumber**    | **int**            |
| Número da Sessão do paciente (Ex.: 1 para a primeira Sessão, 2 para a segunda, etc.) |
| **insertionDate**           | **string**         |
| Data de criação da Sessão                        |
| **updateDate**              | **string**         |
| Data de atualização da Sessão                    |
| **patientId**               | **string**         |
| Identificação anônima do paciente que realizou a Sessão |
| **patientAge**              | **int**            |
| Idade do paciente                                |
| **patientHeight**           | **float**          |
| Altura do paciente                               |
| **patientWeight**           | **float**          |
| Peso do paciente                                 |
| **mainComplaint**           | **string**         |
| Queixa principal do paciente                     |
| **historyOfCurrentDisease** | **string**         |
| Histórico da condição atual do paciente          |
| **historyOfPastDisease**    | **string**         |
| Histórico de condições anteriores do paciente    |
| **diagnosis**               | **string**         |
| Diagnóstico dado ao paciente pelo profissional de saúde |
| **relatedDiseases**         | **string**         |
| Condições relacionadas à do paciente             |
| **medications**             | **string**         |
| Medicações das quais o paciente faz uso          |
| **physicalEvaluation**      | **string**         |
| Avaliação física do paciente realizada pelo profissional |
| **numberOfMovements**       | **int**            |
| Número de Movimentos pertencentes à Sessão       |
| **duration**                | **float**          |
| Duração total da Sessão. Equivale à soma das durações dos seus Movimentos |
| **movements**               | **List<[Movement](#movement)>** |
| Lista dos Movimentos pertencentes à Sessão       |

**Métodos:**
| Método     | Tipo       | Parâmetros              |
| :--------- | :--------- | ----------------------: |
| **ToJson** | **string** | **bool update = false** |
| Converte a Sessão para json. O parâmetro update modifica o json gerado: para requisições de criação use update como `false` e para requisições de atualização use update como `true` |

### Miscelânea
A seguir, estão documentadas as classes relacionadas à desserialização dos objetos json recebidos como respostas do ReBaseRS. Estas classes existem para facilitar a conversão de json para objetos utilizando a biblioteca `Newtonsoft.Json`. Ademais, a classe Serializer é utilizada para centralizar a desserialização das respostas.

#### MetaData
Esta classe modela o objeto `meta` retornado pelo ReBaseRS em requisições de listagem e contém metadados sobre o recurso e sobre paginação.

| Atributo                                     | Tipo    |
| :------------------------------------------- | ------: |
| **current_page (alias: currentPage)**        | **int** |
| **next_page (alias: nextPage)**              | **int** |
| **total_count (alias: totalCount)**          | **int** |
| **total_page_count (alias: totalPageCount)** | **int** |

#### SerializableMovement
Esta classe possui os mesmos atributos da classe `Movement` padrão e não possui nenhum método. Dentro desta classe está a subclasse `AppData`, usadas para modelar o atributos `app`.

| Atributo              | Tipo                                        |
| :-------------------- | ------------------------------------------: |
| **_id (alias: id)**   | **string**                                  |
| **label**             | **string**                                  |
| **description**       | **string**                                  |
| **device**            | **string**                                  |
| **fps**               | **float**                                   |
| **duration**          | **float**                                   |
| **numberOfRegisters** | **int**                                     |
| **articulations**     | **string[]**                                |
| **sessionId**         | **string**                                  |
| **patientId**         | **string**                                  |
| **professionalId**    | **string**                                  |
| **insertionDate**     | **string**                                  |
| **updateDate**        | **string**                                  |
| **app**               | **[AppData](#appdata)**                     |
| **registers**         | **Dictionary<string, float[]>[]**           |
| **id**                | **string**                                  |

##### AppData
| Atributo | Tipo       |
| :------- | ---------: |
| **code** | **int**    |
| **data** | **string** |


#### SerializableSession
Esta classe possui os mesmos atributos da classe `Session` padrão e não possui nenhum método. Dentro desta classe estão as subclasses `PatientData` e `MedicalData`, usadas para modelar os atributos `patient` e `medicalData`.

| Atributo                 | Tipo                                                |
| :----------------------- | --------------------------------------------------: |
| **_id (alias: id)**      | **string**                                          |
| **title**                | **string**                                          |
| **description**          | **string**                                          |
| **professionalId**       | **string**                                          |
| **patientSessionNumber** | **int**                                             |
| **numberOfMovements**    | **int**                                             |
| **duration**             | **float**                                           |
| **insertionDate**        | **string**                                          |
| **updateDate**           | **string**                                          |
| **patient**              | **[PatientData](#patientdata)**                     |
| **medicalData**          | **[MedicalData](#medicaldata)**                     |
| **movements**            | **[SerializableMovement](#serializablemovement)[]** |

##### PatientData
| Atributo   | Tipo       |
| :--------- | ---------: |
| **id**     | **string** |
| **age**    | **int**    |
| **height** | **float**  |
| **weight** | **float**  |

##### MedicalData
| Atributo                    | Tipo       |
| :-------------------------- | ---------: |
| **mainComplaint**           | **string** |
| **historyOfCurrentDisease** | **string** |
| **historyOfPastDisease**    | **string** |
| **diagnosis**               | **string** |
| **relatedDiseases**         | **string** |
| **medications**             | **string** |
| **physicalEvaluation**      | **string** |

### Exceções
Esta API define algumas exceções personalizadas para os modelos de dados e casos de uso do ReBase. São elas:

1. **MismatchedArticulationsException:** disparada ao criar um Movimento com Registros que tenham articulações diferentes das definidas no Movimento ou ao adicionar a um Movimento um Registro que tenha articulações diferentes das do Movimento;
2. **MissingAttributeException:** disparada ao tentar enviar uma requisição ao ReBaseRS e algum parâmetro não tenha um atributo obrigatório;
3. **RepeatedArticulationException:** disparada ao criar um Movimento ou um Registro com uma lista de articulações que contenha articulações repetidas.
