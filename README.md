# Unity ReBase
Este projeto é uma API REST escrita em C# para uso no Unity, para comunicação com o ReBase, um banco de dados um banco de dados Apache Cassandra de sessões de reabilitação física.

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

## Buscando Movimentos e Sessões
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

## Atualizando e deletando
```C#
// São disponibilizados métodos para deletar e excluir Movimentos e Sessões
APIResponse response = await RESTClient.Instance.UpdateMovement(id, updatedMovement);
response = await RESTClient.Instance.DeleteMovement(id);

response = await RESTClient.Instance.UpdateSession(id, updatedSession);
response = await RESTClient.Instance.DeleteSession(id);
```

## Samples:
Este pacote inclui uma cena com alguns exemplos adicionais de utilização da API. A cena e os códigos relacionados se encontram na paste `Samples~/UsageExample`