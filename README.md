# Unity ReBase
Este projeto é uma API REST escrita em C# para uso no Unity, para comunicação com o ReBase, um banco de dados um banco de dados Apache Cassandra de sessões de reabilitação física.

## Visão Geral:
O pacote Unity ReBase contém modelos para Sessões e Movimentos, além de versões serializáveis destas classes. A classe `APIResponse` modela de forma generalizada as respostas enviadas pelo `ReBase REST Server (RRS)`. Todos os modelos se encontram na pasta `Runtime/Models`. Estão presentes também algumas exceções personalizadas, localizadas na pasta `Runtime/Exceptions`. A classe `RESTClient` é responsável pelo envio de requisições ao RRS e se encontra na raiz da paste `Runtime`. Por fim, outras classes diversas encontram-se na pasta `Runtime/Misc`.

## Instalação:
1. Baixe o arquivo .zip;
2. Extraia a pasta compactada;
3. No editor do Unity, [instale o pacote a partir do disco](https://docs.unity3d.com/Manual/upm-ui-local.html).

## Requisitos:
* Este pacote utiliza a biblioteca [Newtonsoft Json 3.2](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@3.2/manual/index.html) e portanto suporta o editor **2018.4 ou superior**.