# ReBase REST API
Este projeto é uma API REST escrita em C# para uso no Unity, para comunicação com o ReBase, um banco de dados um banco de dados Apache Cassandra de sessões de reabilitação física.

Sessões:

Cada sessão de uso do aplicativo é composta por R registros e é guardada em uma documento no Apache Cassandra.

Estrutura de uma sessão no Cassandra:

    {
        "id": <string: id arbitrário da sessão>,
        "title": <string: contém o título da sessão>,
        "device": <string: o dispositivo usado para obter os dados (Kinect, BSM, MediaPipe)>,
        "description": <string: uma descrição da sessão. Pode conter informações além das especificadas nos campos do
                        documento>,
        "professionalid": <string: o ID do profissional da saúde que conduziu a sessão>,
        "patientname": <string: o nome do paciente>,
        "maincomplaint": <string: a queixa principal do paciente>,
        "historyofcurrentdesease": <string: história da doença atual do paciente>,
        "historyofpastdesease": <string: história de doenças passados do paciente>,
        "diagnosis": <string: diagnóstico do paciente>,
        "relateddeseases": <string: doenças relacionadas à do paciente>,
        "medications": <string: medicamentos utilizados pelo paciente>,
        "physicalevaluation": <string: dados subjetivos relevantes ao fisioterapeuta>
        "patientage": <inteiro: a idade do paciente>,
        "patientheight": <float: a altura em centimetros do paciente>,
        "patientweight": <float: o peso em quilogramas do paciente>,
        "patientsessionnumber": <inteiro: o número da sessão>,
        "sessionduration": <float: a duração em minutos da sessão>,
        "numberofregisters" <inteiro: o número de registros contidos no documento>,
        "artindexpattern": <string: representa os indíces das articulações dos registros na lista>
        "sessiondata": <binary: a lista comprimida de registros codificada como string Base64, onde estão as coordenadas das articulações>
    }

Registros:

Cada sessão é composta de R registros que são serializados antes de serem enviados para o servidor. Cada registro contém as coordenadas x, y e z de cada uma das N articulações num determinado frame F.

Estrutura de um registro:

    {
        articulation1 = [x, y, z];
        articulation2 = [x, y, z];
        ...
        articulation20 = [x, y, z]
    }

As sessões armazenam a lista serializada de registros.
O armazenamento da lista de registros:

Inicialmente, os dados das articulações seriam gravados em texto plano, no entanto, esse modelo se mostrou muito pesado, excedendo o limite de 16 MB por documento do MongoDB. Para tentar contornar esse problema, primeiro, comprimimos os dados utilizando a classe DeflateStream do C# e os gravamos em bytes no banco. Como, ainda assim, o limite foi excedido, decidimos converter os bytes comprimidos para uma string Base64, o que ainda assim não resolveu o problema. O próximo passo foi limitar a duas o número de casas decimais das coordenadas das articulações e, enfim, visto que o problema ainda persistia, decidimos remover completamente os índices da lita de articulações.

Desta forma, dentro de cada sessão foi adicionado o campo "binary", no qual se encontram os bytes dos dados das articulações. O campo "artIndexPattern" foi adicionado para suprir a falta de índices na lista de registros.
Estrutura do campo "artIndexPattern":

Este campo contém o padrão dos índices das articulações e mostra explicitamente quais são as articulações aN contidas naquela lista e sua ordem. Dessa forma, uma sessão que contém N articulações, terá o padrão "a1;a2;a3;...;aN".
Estrutura do campo "binary":

O campo "binary" contém os bytes da lista de registro, que contém as coordenadas das articulações.

A lista de registros é uma string que foi comprimida e convertida para Base64, desta forma, após sua recuperação, ela precisa ser convertida de bytes para uma string Base64, que deverá então ser convertida para um novo array de bytes que será, por fim, convertida para a string UTF-8 original, a lista serializada de registros daquela sessão.

    Exemplo de uma lista de 2 registros de 3 articulações: 0,0,0;1,2,3;4,7,8/1,0,0;1,1,3;4,7,10

A string contendo a lista de registros da sessão deve passar por um parsing após sua recuperação. Cada registro está separado por "/" e, dentro de cada registro, as articulações estão separadas por ";". Cada uma das coordenadas de uma articulação são separadas por ",".

Como dito antes, não há índices na lista que indiquem a qual articulação cada tripla de coordenadas pertence; esta informação é dada implicitamente pelo padrão das articulações, mencionado anteriormente na sessão anterior.
