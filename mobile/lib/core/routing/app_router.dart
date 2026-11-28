import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:healthcare_app/core/di/dependency_injection.dart';
import 'package:healthcare_app/features/auth/presentation/cubit/login_cubit.dart';
import 'package:healthcare_app/features/auth/presentation/cubit/register_patient_cubit.dart';
import 'package:healthcare_app/features/auth/presentation/screens/login_screen.dart';
import 'package:healthcare_app/features/auth/presentation/screens/register_screen.dart';
import 'package:healthcare_app/features/choose_language/presentation/screens/choose_language_screen.dart';
import 'package:healthcare_app/features/home_screen.dart';
import 'package:healthcare_app/features/main_layout/presentation/main_layout_screen.dart';
import '../../features/onboarding/presentation/screens/onboarding_screen.dart';
import '../../features/splash/presentation/screens/splash_screen.dart';
import 'routes.dart';

class AppRouter {
  Route? generateRoute(RouteSettings settings) {
    switch (settings.name) {
      case Routes.splashScreen:
        return MaterialPageRoute(builder: (_) => const SplashScreen());
      case Routes.onBoardingScreen:
        return MaterialPageRoute(builder: (_) => const OnboardingScreen());
      case Routes.chooseLanguageScreen:
        return MaterialPageRoute(builder: (_) => const ChooseLanguageScreen());
      case Routes.loginScreen:
        return MaterialPageRoute(
          builder: (_) => BlocProvider(
            create: (_) => getIt<LoginCubit>(),
            child: const LoginScreen(),
          ),
        );
      case Routes.homeScreen:
        final initialIndex = settings.arguments as int? ?? 0;
        return MaterialPageRoute(
          builder: (_) => MainLayoutScreen(initialIndex: initialIndex),
        );
      case Routes.registerScreen:
        return MaterialPageRoute(
          builder: (_) => BlocProvider(
            create: (_) => getIt<RegisterPatientCubit>(),
            child: const RegisterScreen(),
          ),
        );
      default:
        return null;
    }
  }
}
