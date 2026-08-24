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
       
       stage('check docker')
       {
           steps
           {
               withCredentials([usernamePassword(credentialsId: 'dockerhub-devopslab', passwordVariable: 'DOCKER_TOKEN', usernameVariable: 'DOCKER_USERNAME')]) {
   sh '''
                echo "$DOCKER_TOKEN" | docker login \
                    --username "$DOCKER_USERNAME" \
                    --password-stdin
            '''
}
               
           }
           
       }
       
       
      stage('checkout') {
         steps {
             
             deleteDir()
             
            git branch: 'main', credentialsId: 'GitHub', url: 'https://github.com/ipushprajmishra/DevopsLabApi.git'
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
        script {
            def gitSha = sh(
                script: 'git rev-parse --short HEAD',
                returnStdout: true
            ).trim()

            sh """
                docker build \
                    -t ${DOCKER_IMAGE}:${gitSha} \
                    .
            """
        }
    }
}
stage('Docker Push') {
    steps {
 script {
            def gitSha = sh(
                script: 'git rev-parse --short HEAD',
                returnStdout: true
            ).trim()
           sh """


                docker push ${DOCKER_IMAGE}:${gitSha}

                docker logout
           """
 }
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

// stage('Deploy') {
//     when {
//         expression {
//             params.DEPLOY_ENV == 'dev'
//         }
//     }
//     steps {
//         sh '''
//             rm -rf /opt/devopslab-api/*
//             cp -r publish/* /opt/devopslab-api/
//             sudo systemctl restart ${APP_NAME}
//         '''
//     }
// }


stage('Docker Deploy') {
    when {
        expression {
            params.DEPLOY_ENV == 'dev'
        }
    }

    steps {
        sh '''
            export IMAGE_TAG="$GIT_SHA"

            docker compose \
                -f docker-compose.yml \
                -f docker-compose.deploy.yml \
                pull api

            docker compose \
                -f docker-compose.yml \
                -f docker-compose.deploy.yml \
                up -d api
        '''
    }
}

   }
}