# Unity ReBase
Este projeto é uma API REST escrita em C# para uso no Unity, para comunicação com o ReBase, um banco de dados um banco de dados Apache Cassandra de sessões de reabilitação física.

## Índice
- [Unity ReBase](#unity-rebase)
  - [Índice](#índice)
  - [Visão Geral:](#visão-geral)
  - [Instalação:](#instalação)
  - [Requisitos:](#requisitos)
  - [Quickstart:](#quickstart)
    - [Criando um Movimento:](#criando-um-movimento)
    - [Criando uma Sessão](#criando-uma-sessão)
    - [Buscando Movimentos e Sessões](#buscando-movimentos-e-sessões)
    - [Atualizando e deletando](#atualizando-e-deletando)
  - [Documentação](#documentação)
    - [Modelos](#modelos)
      - [APIResponse](#apiresponse)
        - [Atributos](#atributos)
      - [Movement](#movement)
        - [Atributos](#atributos-1)
        - [Métodos](#métodos)
      - [Register](#register)
        - [Atributos](#atributos-2)
        - [Métodos](#métodos-1)
      - [Rotation](#rotation)
        - [Atributos](#atributos-3)
        - [Métodos](#métodos-2)
      - [Session](#session)
        - [Atributos](#atributos-4)
        - [Métodos](#métodos-3)
  - [Samples:](#samples)


## Visão Geral:
O pacote Unity ReBase contém modelos para Sessões e Movimentos, além de versões serializáveis destas classes. A classe `APIResponse` modela de forma generalizada as respostas enviadas pelo `ReBase REST Server (RRS)`. Todos os modelos se encontram na pasta `Runtime/Models`. Estão presentes também algumas exceções personalizadas, localizadas na pasta `Runtime/Exceptions`. A classe `RESTClient` é responsável pelo envio de requisições ao RRS e se encontra na raiz da paste `Runtime`. Por fim, outras classes diversas encontram-se na pasta `Runtime/Misc`.

## Instalação:
1. Baixe o arquivo .zip;
2. Extraia a pasta compactada;
3. No editor do Unity, [instale o pacote a partir do disco](https://docs.unity3d.com/Manual/upm-ui-local.html).

## Requisitos:
* Este pacote utiliza a biblioteca [Newtonsoft Json 3.2](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@3.2/manual/index.html) e portanto suporta o editor **2018.4 ou superior**;
* Este pacote também depende da classe `System.Net.Http` e do paradigma `Async/Await`, por isso requer pelo menos **C# 5** e [uma destas versões do pacote .NET](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-7.0#applies-to).

## Quickstart:
Para utilzar a API, basta incluir a biblioteca adicionando `using ReBase` no começo do seu arquivo .cs

### Criando um Movimento:
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

// Utilize a classe singleton RESTClient para inserir o movimento
// Todas as requisições são assíncronas.
APIResponse response = await RESTClient.Instance.InsertMovement(movement);
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
            articulationData: new Register[1] {
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
            articulationData: new Register[1] {
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

APIResponse response = await RESTClient.Instance.InsertSession(session);
```

### Buscando Movimentos e Sessões
```C#
// É possível encontrar um único Movimento ou listar vários
APIResponse response = await RESTClient.Instance.FindMovement(id);
Debug.Log($"Movimento: {response.movement}");

// A listagem permite filtros e suporta paginação
// Os filtros possívels são: professionalId (id do profissional de saúde), patientId (id do paciente),
// movementLabel (identificação do movimento), articulations (articulações incluídas no movimento)
response = await RESTClient.Instance.FetchMovements(professionalId: "professional", patientId: "patient", page: 1, per: 10);
Debug.Log($"Movimentos: {response.movements}");

// Da mesma forma, as Sessões podem ser buscas individualmente ou listadas
response = await RESTClient.Instance.FindSession(id);
Debug.Log($"Sessão: {response.session}");

// Os filtros suportados pela listagem de Sessão são: professionalId e patientId.
// Também são aceitos os filtros movementLabel e articulations, mas estes filtram os movimentos das Sessões
response = await RESTClient.Instance.FetchSessions(professionalId: "professional", patientId: "patient", page: 1, per: 10);
Debug.Log($"Sessões: {response.sessions}");
```

### Atualizando e deletando
```C#
// São disponibilizados métodos para deletar e excluir Movimentos e Sessões
APIResponse response = await RESTClient.Instance.UpdateMovement(id, updatedMovement);
response = await RESTClient.Instance.DeleteMovement(id);

response = await RESTClient.Instance.UpdateSession(id, updatedSession);
response = await RESTClient.Instance.DeleteSession(id);
```

## Documentação
A seguir, estão incluídas tabelas e descrições detalhando todas as classes da API Unity ReBase.

### Modelos

#### APIResponse
A classe APIResponse modela uma resposta generalizada do servidor RRS.

##### Atributos
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
| Apresenta possíveis inconsistências encontradas na requisição pelo RRS, mas que não são críticas a ponto de provocar um erro. Por exemplo, ao criar um Movimento, caso algum campo enviado esteja vazio o RRS completará a requisição, mas retornará um warning correspondente avisando que o campo em questão foi excluído |
| **movements**    | **SerializableMovement[]** |
| Possível lista de Movimentos retornada pelo RRS |
| **movement**     | **SerializableMovement**   |
| Possível Movimento retornado pelo RRS |
| **sessions**     | **SerializableSession[]**  |
| Possível lista de Sessão retornada pelo RRS |
| **session**      | **SerializableSession**    |
| Possível Sessão retornada pelo RRS**    |
| **deletedId**    | **string**                 |
| Em requisições do tipo DELETE, esta propriedada contém o ID do documento excluído |
| **deletedCount** | **int**                    |
| Em requisições do tipo DELETE, contém o número de documentos excluídos. Por exemplo, ao deletar uma Sessão, é possível também excluir todos os seus Movimentos |

#### Movement
Modela um Movimento do ReBase.

##### Atributos
| Atributo              | Tipo               |
| :-------------------- | -----------------: |
| **id**                | **string**         |
| ID do Movimento                            |
| **label**             | **string**         |
| Label, ou etiqueta, de identificação do Movimento. Normalmente representa uma categoria ou grupo de Movimentos |
| **description**       | **string**         |
| Descrição do Movimento                     |
| **device**            | **string**         |
| Dispositivo responsável pela captura do Movimento (E.g. Kineck, MediaPipe, etc.) |
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
| **articulationData**  | **List<Register>** |
| Lista de registros que representam o Movimento em si |

##### Métodos
| Método                       | Tipo            | Parâmetros                         |
| :--------------------------- | :-------------- | ---------------------------------: |
| **AddRegister**              | **void**        | **Register register**              |
| Adiciona um novo Registro ao Movimento                                              |
| **ToJson**                   | **string**      | **bool update (default: false)**   |
| Converte o Movimento para json. O parâmetro update modifica o json gerado: para requisições de criação use update como `false` e para requisições de atualização use update como `true` |
| **ToCreateSessionJson**      | **string**      |                                    |
| Converte o Movimento para o json a ser utilizado ao criar uma Sessão com Movimentos |
| **CompareArticulationLists** | **static bool** | **string[] listA, string[] listB** |
| Compara duas listas de Articulações e retorna `true` se forem iguais ou `false` se forem diferentes |

#### Register
Modela um Registro do ReBase, essencialmente um quadro, ou um frame, de um Movimento. Um Registro contém as rotações nos eixos x, y e z de cada Articulação do Movimento em um determinado instante.

##### Atributos
| Atributo              | Tipo               |
| :-------------------- | -----------------: |
| **articulationCount** | **int**            |
| A quantidade de Articulações presentes no Registro |
| **articulations**     | **string[]**       |
| A lista das Articulações presentes no Registro |
| **isEmpty**           | **bool**           |
| Indica se o registro está vazio, ou seja, não contem nenhuma Articulação |

##### Métodos
| Método                       | Tipo         | Parâmetros                                  |
| :--------------------------- | :----------- | ------------------------------------------: |
| **SetArticulations**         | **void**     | **string[] articulationList**               |
| Inicializa as Articulações do registro a partir de uma lista de Articulações              |
| **SetArticulationRotations** | **void**     | **string articulation, Rotation rotations** |
| Atribui Rotações a uma Articulação específica                                             |
| **GetArticulationRotations** | **Rotation** | **string articulation**                     |
| Rotorna as Rotações de uma Articulação específica                                         |

#### Rotation
Representa um conjunto de rotações de uma Articulação.

##### Atributos
| Atributo | Tipo       |
| :------- | ---------: |
| **x**    | **double** |
| A rotação no eixo X   |
| **y**    | **double** |
| A rotação no eixo Y   |
| **z**    | **double** |
| A rotação no eixo Z   |

##### Métodos
| Método        | Tipo        | Parâmetros |
| :------------ | :---------- | ---------: |
| **ToVector3** | **Vector3** |            |
| Converte a Rotação para um objeto da classe ``UnityEngine.Vector3`` |

#### Session
Modela uma Sessão do ReBase.

##### Atributos
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
| **historyOfCurrentDesease** | **string**         |
| Histórico da condição atual do paciente          |
| **historyOfPastDesease**    | **string**         |
| Histórico de condições anteriores do paciente    |
| **diagnosis**               | **string**         |
| Diagnóstico dado ao paciente pelo profissional de saúde |
| **relatedDeseases**         | **string**         |
| Condições relacionadas à do paciente             |
| **medications**             | **string**         |
| Medicações das quais o paciente faz uso          |
| **physicalEvaluation**      | **string**         |
| Avaliação física do paciente realizada pelo profissional |
| **numberOfMovements**       | **int**            |
| Número de Movimentos pertencentes à Sessão       |
| **duration**                | **float**          |
| Duração total da Sessão. Equivale à soma das durações dos seus Movimentos |
| **movements**               | **List<Movement>** |
| Lista dos Movimentos pertencentes à Sessão       |

##### Métodos
| Método     | Tipo       | Parâmetros              |
| :--------- | :--------- | ----------------------: |
| **ToJson** | **string** | **bool update = false** |
| Converte a Sessãi para json. O parâmetro update modifica o json gerado: para requisições de criação use update como `false` e para requisições de atualização use update como `true` |

## Samples:
Este pacote inclui uma cena com alguns exemplos adicionais de utilização da API. A cena e os códigos relacionados se encontram na paste `Samples~/UsageExample`
