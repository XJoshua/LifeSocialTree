set GEN_CLIENT=dotnet ..\Tools\Luban.ClientServer\Luban.ClientServer.dll
set GEN_JSON_OUTPUT=../Assets/Resources/TextAsset/JsonConfig
set GEN_CODE_OUTPUT=../Assets/Scripts/TBConfig

%GEN_CLIENT% -j cfg --^
 -d Defines\__root__.xml ^
 --input_data_dir Datas ^
  --output_data_dir %GEN_JSON_OUTPUT% ^
 --output_code_dir %GEN_CODE_OUTPUT% ^
 --gen_types code_cs_unity_json,data_json ^
 -s all
pause