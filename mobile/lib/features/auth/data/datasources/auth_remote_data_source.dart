import 'package:dio/dio.dart';
import 'package:healthcare_app/core/networking/api_constants.dart';
import 'package:healthcare_app/features/auth/data/models/login_request_model.dart';
import 'package:healthcare_app/features/auth/data/models/login_response_model.dart';
import 'package:healthcare_app/features/auth/data/models/register_patient_request_model.dart';
import 'package:retrofit/retrofit.dart';

part 'auth_remote_data_source.g.dart';

@RestApi(baseUrl: ApiConstants.baseUrl)
abstract class AuthRemoteDataSource {
  factory AuthRemoteDataSource(Dio dio, {String baseUrl}) =
      _AuthRemoteDataSource;

  @POST(ApiConstants.login)
  Future<LoginResponseModel> login(@Body() LoginRequestModel loginRequestBody);

  @POST(ApiConstants.registerPatient)
  Future<LoginResponseModel> registerPatient(
    @Body() RegisterPatientRequestModel registerRequestBody,
  );
}
