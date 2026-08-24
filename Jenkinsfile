pipeline {
  agent any

  options {
    skipDefaultCheckout(true)
  }

  parameters {
    choice choices: ['dev', 'staging', 'production'], description: 'Environment to deploy the application to', name: 'DEPLOY_ENV'

  }

  environment {
    APP_NAME = 'devopslab-api'
    DOCKER_IMAGE = 'ipushprajmishra/devopslab-api'
  }
  stages {

    stage('check docker') {
      steps {
        withCredentials([usernamePassword(credentialsId: 'dockerhub-devopslab', passwordVariable: 'DOCKER_TOKEN', usernameVariable: 'DOCKER_USERNAME')]) {
          sh '''
          echo "$DOCKER_TOKEN" | docker login\
            --username "$DOCKER_USERNAME"\
            --password - stdin '''
        }

      }

    }

    stage('checkout') {
      steps {

        deleteDir()

        git branch: 'main', credentialsId: 'GitHub', url: 'https://github.com/ipushprajmishra/DevopsLabApi.git'

        script {
          env.GIT_SHA = env.GIT_COMMIT.take(6)
        }
      }
    }
    stage('Restore') {
      steps {
        sh 'dotnet restore DevopsLabApi.sln'
      }

    }
    stage('Build') {
      steps {
        sh 'dotnet build DevopsLabApi.sln --configuration Release --no-restore'
      }
    }
    stage('Test') {
      steps {
        sh 'dotnet test DevopsLabApi.sln --configuration Release --no-build'
      }
    }
    stage('Publish') {
      steps {
        sh 'dotnet publish DevopsLabApi/DevopsLabApi.csproj --configuration Release --no-build --output publish'
      }
    }
    stage('Docker Build') {
      steps {

        sh """
        docker build\
          -
          t $ {
            DOCKER_IMAGE
          }: $ {
            GIT_SHA
          }\
          .
        """

      }
    }
    stage('Docker Push') {
      steps {

        sh """

        docker push $ {
          DOCKER_IMAGE
        }: $ {
          GIT_SHA
        }

        docker logout
          """

      }
    }

    stage('Archive Artifact') {
      steps {
        archiveArtifacts artifacts: 'publish/**', fingerprint: true
      }
    }
    stage('CI Complete') {
      steps {
        echo 'CI completed successfully. Artifact is ready for deployment.'
      }
    }
    stage('Show Environment') {
      steps {
        echo "Deploying to environment: ${params.DEPLOY_ENV}"
      }
    }

    stage('Docker Deploy') {
      when {
        expression {
          params.DEPLOY_ENV == 'dev'
        }
      }

      steps {
        sh '''
        export IMAGE_TAG = "$GIT_SHA"

        docker compose\
          -
          f docker - compose.yml\ -
          f docker - compose.deploy.yml\
        pull api

        docker compose\
          -
          f docker - compose.yml\ -
          f docker - compose.deploy.yml\
        up - d api '''
      }
    }

  }
}