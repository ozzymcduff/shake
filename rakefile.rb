
require 'albacore'

task :default => ['build']

def nunit_cmd()
  return Dir.glob(File.join(File.dirname(__FILE__),"src","packages","NUnit.Runners.*","tools","nunit-console.exe")).first
end
dir = File.dirname(__FILE__)
desc "build using msbuild"
msbuild :build do |msb|
  msb.properties :configuration => :Debug
  msb.targets :Clean, :Rebuild
  msb.verbosity = 'quiet'
  msb.solution =File.join(dir,"src", "Shake.sln")
end
desc "test using nunit console"
nunit :test => :build do |nunit|
  nunit.command = nunit_cmd()
  nunit.assemblies File.join(dir,"src","ItUp/bin/Debug/Shake.It.Up.dll")
end

task :core_copy_to_nuspec => [:build] do
  output_directory_lib = File.join(dir,"nuget/lib/40/")
  mkdir_p output_directory_lib
  cp Dir.glob("./src/Shake/bin/Debug/Shake.dll"), output_directory_lib
end

desc "create the nuget package"
task :nugetpack => [:core_nugetpack]

def nuget_exe
  ".\\src\\.nuget\\NuGet.exe"
end

nugetpack :core_nugetpack => [:core_copy_to_nuspec] do |nuget|
  nugetfolder = File.join(dir,"nuget")
  nuget.command = nuget_exe
  nuget.base_folder = nugetfolder
  nuget.output = nugetfolder
  nuget.nuspec = File.join(nugetfolder,'Shake.nuspec')
end

desc "Install missing NuGet packages."
exec :install_packages do |cmd|
  FileList["src/**/packages.config"].each do |filepath|
    cmd.command = "./src/.nuget/NuGet.exe"
    cmd.parameters = "i #{filepath} -o ./src/packages"
  end
end

namespace :mono do
  dir = File.dirname(__FILE__)
  desc "build shake on mono"
  xbuild :build do |msb|
    msb.properties :configuration => :Debug
    msb.targets :Clean, :Rebuild
    msb.verbosity = 'quiet'
    msb.solution =File.join(dir,"src", "Shake.sln")
  end

  desc "Install missing NuGet packages."
  task :install_packages do |cmd|
    FileList["src/**/packages.config"].each do |filepath|
      sh "mono ./src/.nuget/NuGet.exe i #{filepath} -o ./src/packages"
    end
  end

end


