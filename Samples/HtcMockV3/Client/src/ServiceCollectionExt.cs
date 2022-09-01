﻿// This file is part of the ArmoniK project
// 
// Copyright (C) ANEO, 2021-2022.
//   W. Kirschenmann   <wkirschenmann@aneo.fr>
//   J. Gurhem         <jgurhem@aneo.fr>
//   D. Dubuc          <ddubuc@aneo.fr>
//   L. Ziane Khodja   <lzianekhodja@aneo.fr>
//   F. Lemaitre       <flemaitre@aneo.fr>
//   S. Djebbar        <sdjebbar@aneo.fr>
//   J. Fonseca        <jfonseca@aneo.fr>
//   D. Brasseur       <dbrasseur@aneo.fr>
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using ArmoniK.Api.gRPC.V1;

using Grpc.Core;
using Grpc.Net.Client;

using JetBrains.Annotations;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ArmoniK.Samples.HtcMock.Client
{
  public static class ServiceCollectionExt
  {
    [PublicAPI]
    public static IServiceCollection AddComponents(this IServiceCollection serviceCollection,
                                                   IConfiguration          configuration)
    {
      serviceCollection.Configure<Adapter.Options.Grpc>(configuration.GetSection(Adapter.Options.Grpc.SettingSection))
                       .AddSingleton(sp =>
                                     {
                                       var options = sp.GetRequiredService<IOptions<Adapter.Options.Grpc>>();
                                       return GrpcChannel.ForAddress(options.Value.Endpoint);
                                     })
                       .AddTransient(sp =>
                                     {
                                       ChannelBase channel = sp.GetRequiredService<GrpcChannel>();
                                       return new Submitter.SubmitterClient(channel);
                                     })
                       .AddSingleton<GridClient>();

      return serviceCollection;
    }
  }
}
