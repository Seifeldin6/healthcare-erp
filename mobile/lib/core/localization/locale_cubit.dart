import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:healthcare_app/core/storage/shared_pref_helper.dart';
import 'package:healthcare_app/core/storage/storage_keys.dart';

class LocaleCubit extends Cubit<Locale> {
  final SharedPrefHelper _sharedPrefHelper;

  LocaleCubit(this._sharedPrefHelper)
      : super(_loadInitialLocale(_sharedPrefHelper));

  static Locale _loadInitialLocale(SharedPrefHelper helper) {
    final savedCode = helper.getString(StorageKeys.appLocale);
    if (savedCode == 'ar' || savedCode == 'en') {
      return Locale(savedCode!);
    }
    return const Locale('en');
  }

  Future<void> changeLocale(String languageCode) async {
    if (state.languageCode == languageCode) return;
    await _sharedPrefHelper.setString(StorageKeys.appLocale, languageCode);
    emit(Locale(languageCode));
  }
}