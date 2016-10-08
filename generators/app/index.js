'use strict';
var yeoman = require('yeoman-generator');
var chalk = require('chalk');
var yosay = require('yosay');
var Guid = require('Guid');

module.exports = yeoman.Base.extend(
  {
    prompting: function () {
      // Have Yeoman greet the user.
      this.log(yosay(
        'Welcome to the peachy ' + chalk.red('generator-aspnet-core-api') + ' generator!'
      ));

      var prompts = [{
        type: 'input',
        name: 'name',
        message: 'Your project name',
        default: this.appname // Default to current folder name
      }];

      return this.prompt(prompts).then(function (props) {
        this.props = props;
      }.bind(this));
    },

    writing: function () {
      var projects = {
        baseName: this.props.name.replace(' ', '.'),
        data: {
          guid: Guid.raw()
        },
        services: {
          guid: Guid.raw()
        },
        api: {
          guid: Guid.raw()
        },
        utils:
        {
          guid: Guid.raw()
        }
      }


      // copy solution
      this.fs.copyTpl(
        this.templatePath('ProjectTemplate.sln'),
        this.destinationPath(projects.baseName + '.sln'),
        projects
      );

      // Data Project 
      this._coreProjectsTemplate(projects);
      this._rootProjectsTemplate(projects);
    },

    _coreProjectsTemplate: function (projects) {

      this.mkdir("Core");

      this.fs.copyTpl(
        this.templatePath("Core/VersionInfo.cs"),
        this.destinationPath("Core/VersionInfo.cs"),
        projects
      );

      var projectName = projects.baseName + ".Data";

      this.mkdir("Core/" + projectName);

      this.fs.copyTpl(
        this.templatePath("Core/ProjectTemplate.Data.csproj"),
        this.destinationPath("Core/" + projectName + "/" + projectName + ".csproj"),
        projects
      );
      this.fs.copyTpl(
        this.templatePath('Core/Data/**/*'),
        this.destinationPath("Core/" + projectName),
        projects
      );
      //---------------------------------------------------------------------------------------
      var projectName = projects.baseName + ".Services";
      this.mkdir("Core/" + projectName);

      this.fs.copyTpl(
        this.templatePath("Core/ProjectTemplate.Services.csproj"),
        this.destinationPath("Core/" + projectName + "/" + projectName + ".csproj"),
        projects
      );
      this.fs.copyTpl(
        this.templatePath('Core/Services/**/*'),
        this.destinationPath("Core/" + projectName),
        projects
      );
      //---------------------------------------------------------------------------------------
      var projectName = projects.baseName + ".Api";
      this.mkdir("Core/" + projectName);

      this.fs.copyTpl(
        this.templatePath("Core/ProjectTemplate.Api.csproj"),
        this.destinationPath("Core/" + projectName + "/" + projectName + ".csproj"),
        projects
      );
      this.fs.copyTpl(
        this.templatePath('Core/Api/**/*'),
        this.destinationPath("Core/" + projectName),
        projects
      );
    },
    _rootProjectsTemplate: function (projects)
    {
      var projectName = projects.baseName + ".Utilities";
      this.mkdir(projectName);

      this.fs.copyTpl(
        this.templatePath("ProjectTemplate.Utilities.csproj"),
        this.destinationPath(projectName + "/" + projectName + ".csproj"),
        projects
      );
      this.fs.copyTpl(
        this.templatePath('Utilities/**/*'),
        this.destinationPath(projectName),
        projects
      );
    },

    install: function () {

    }
  });
