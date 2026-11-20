import 'package:dio/dio.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:get_it/get_it.dart';
import 'package:healthcare_app/core/localization/locale_cubit.dart';
import 'package:healthcare_app/core/networking/dio_factory.dart';
import 'package:healthcare_app/core/storage/secure_storage_helper.dart';
import 'package:healthcare_app/core/storage/shared_pref_helper.dart';
import 'package:healthcare_app/features/auth/data/datasources/auth_remote_data_source.dart';
import 'package:healthcare_app/features/auth/data/repositories/auth_repo_impl.dart';
import 'package:healthcare_app/features/auth/domain/repositories/auth_repo.dart';
import 'package:healthcare_app/features/auth/domain/usecases/login_use_case.dart';
import 'package:healthcare_app/features/auth/presentation/cubit/login_cubit.dart';
import 'package:shared_preferences/shared_preferences.dart';

final getIt = GetIt.instance;

Future<void> setupGetIt() async {
  // External
  final sharedPreferences = await SharedPreferences.getInstance();
  getIt.registerSingleton<SharedPreferences>(sharedPreferences);
  getIt.registerLazySingleton<FlutterSecureStorage>(
    () => const FlutterSecureStorage(),
  );

  // Storage
  getIt.registerLazySingleton<SharedPrefHelper>(
    () => SharedPrefHelper(getIt<SharedPreferences>()),
  );
  getIt.registerLazySingleton<SecureStorageHelper>(
    () => SecureStorageHelper(getIt<FlutterSecureStorage>()),
  );

  // Networking
  final dio = await DioFactory.getDio(getIt<SecureStorageHelper>());
  getIt.registerSingleton<Dio>(dio);

  // Core Cubits
  getIt.registerLazySingleton<LocaleCubit>(
    () => LocaleCubit(getIt<SharedPrefHelper>()),
  );

  // Auth Feature
  getIt.registerLazySingleton<AuthRemoteDataSource>(
    () => AuthRemoteDataSource(getIt<Dio>()),
  );
  getIt.registerLazySingleton<AuthRepo>(
    () => AuthRepoImpl(
      getIt<AuthRemoteDataSource>(),
      getIt<SecureStorageHelper>(),
    ),
  );
  getIt.registerLazySingleton<LoginUseCase>(
    () => LoginUseCase(getIt<AuthRepo>()),
  );
  getIt.registerFactory<LoginCubit>(
    () => LoginCubit(getIt<LoginUseCase>()),
  );
}