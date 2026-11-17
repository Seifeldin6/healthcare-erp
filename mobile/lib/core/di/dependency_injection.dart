import 'package:get_it/get_it.dart';
import 'package:healthcare_app/core/localization/locale_cubit.dart';
import 'package:healthcare_app/core/storage/shared_pref_helper.dart';
import 'package:shared_preferences/shared_preferences.dart';

final getIt = GetIt.instance;

Future<void> setupGetIt() async {
  // External
  final sharedPreferences = await SharedPreferences.getInstance();
  getIt.registerSingleton<SharedPreferences>(sharedPreferences);
  // Storage
  getIt.registerLazySingleton<SharedPrefHelper>(
    () => SharedPrefHelper(getIt<SharedPreferences>()),
  );

  // Core Cubits
  getIt.registerLazySingleton<LocaleCubit>(
    () => LocaleCubit(getIt<SharedPrefHelper>()),
  );
}
