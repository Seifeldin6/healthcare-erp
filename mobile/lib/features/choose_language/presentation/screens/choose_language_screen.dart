import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:healthcare_app/core/helpers/extensions.dart';
import 'package:healthcare_app/core/localization/locale_cubit.dart';
import 'package:healthcare_app/core/routing/routes.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/features/choose_language/presentation/widgets/choose_language_header.dart';
import 'package:healthcare_app/features/choose_language/presentation/widgets/choose_language_sheet.dart';

class ChooseLanguageScreen extends StatelessWidget {
  const ChooseLanguageScreen({super.key});

  void _onLanguageChosen(BuildContext context, String languageCode) async {
    await context.read<LocaleCubit>().changeLocale(languageCode);
    if (!context.mounted) return;
    context.pushReplacementNamed(Routes.onBoardingScreen);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.primary,
      body: Column(
        children: [
          const Expanded(child: ChooseLanguageHeader()),
          ChooseLanguageSheet(
            onSelectArabic: () => _onLanguageChosen(context, 'ar'),
            onSelectEnglish: () => _onLanguageChosen(context, 'en'),
          ),
        ],
      ),
    );
  }
}
